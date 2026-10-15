#!/usr/bin/env python3
import argparse
import json
import re
import shutil
from pathlib import Path
import cv2
from tqdm import tqdm
from sklearn.model_selection import train_test_split


def get_frame_num(filename: str) -> int:
    match = re.search(r'\d+', filename)
    return int(match.group()) if match else 0


def process_split(files_list, coco_data, out_img_dir, out_anno_dir, out_orig_anno_dir, n_prev):
    next_img_id = len(coco_data['images']) + 1
    next_ann_id = len(coco_data['annotations']) + 1

    for idx, item in enumerate(files_list):
        subdir = Path(item['image']).parent.name
        unique_name = f"{subdir}_{item['filename']}"
        
        # Копирование файлов
        shutil.copy2(item['image'], out_img_dir / unique_name)
        shutil.copy2(item['annotation'], out_orig_anno_dir / Path(item['annotation']).name)

        # Получение размеров изображения
        img = cv2.imread(str(item['image']))
        if img is None:
            print(f"[!] Пропуск: не удалось прочитать {item['image']}")
            continue
        h, w = img.shape[:2]

        # Формирование списка предыдущих кадров
        start_idx = max(0, idx - n_prev)
        prev_imgs = [
            f"{Path(f['image']).parent.name}_{f['filename']}" 
            for f in files_list[start_idx:idx]
        ]

        coco_data['images'].append({
            "id": next_img_id,
            "file_name": unique_name,
            "width": w,
            "height": h,
            "prev_imgs": prev_imgs
        })

        # Обработка аннотаций (конвертация из относительных в абсолютные координаты)
        with open(item['annotation'], 'r', encoding='utf-8') as f:
            ann_data = json.load(f)

        for ann in ann_data.get('annotations', []):
            bbox = ann.get('bbox', {})
            x = bbox.get('x', 0) * w
            y = bbox.get('y', 0) * h
            bw = bbox.get('width', 0) * w
            bh = bbox.get('height', 0) * h

            if bw <= 0 or bh <= 0:
                continue

            coco_data['annotations'].append({
                "id": next_ann_id,
                "image_id": next_img_id,
                "category_id": 1,
                "bbox": [round(x), round(y), round(bw), round(bh)],
                "area": round(bw * bh),
                "segmentation": [],
                "iscrowd": 0
            })
            next_ann_id += 1
        
        next_img_id += 1


def main():
    parser = argparse.ArgumentParser(description="Конвертация синтетических данных Unity в формат Time-COCO")
    parser.add_argument("--root_dir", type=str, required=True, help="Путь к исходной директории с подпапками Unity")
    parser.add_argument("--output_dir", type=str, required=True, help="Путь для сохранения результата")
    parser.add_argument("--train_ratio", type=float, default=0.8, help="Доля данных для обучающей выборки")
    parser.add_argument("--n_prev", type=int, default=5, help="Количество предыдущих кадров в prev_imgs")
    args = parser.parse_args()

    root = Path(args.root_dir).resolve()
    out = Path(args.output_dir).resolve()
    
    if not root.is_dir():
        print(f"Ошибка: директория не найдена: {root}", file=sys.stderr)
        return

    # Создание структуры
    (out / "train2017").mkdir(parents=True, exist_ok=True)
    (out / "val2017").mkdir(parents=True, exist_ok=True)
    (out / "annotations").mkdir(parents=True, exist_ok=True)
    (out / "original_annotations").mkdir(parents=True, exist_ok=True)

    # Сбор данных
    subdirs_data = {}
    for subdir in root.iterdir():
        if not subdir.is_dir():
            continue
        
        files = []
        for json_file in subdir.glob("*.json"):
            try:
                with open(json_file, 'r', encoding='utf-8') as f:
                    data = json.load(f)
                
                img_rel_path = data.get('screenshotPath', '')
                img_filename = Path(img_rel_path).name
                img_path = subdir / img_filename

                if img_path.exists():
                    files.append({
                        'image': img_path,
                        'annotation': json_file,
                        'filename': img_filename
                    })
            except (json.JSONDecodeError, UnicodeDecodeError):
                print(f"[!] Пропуск некорректного JSON: {json_file}")

        if files:
            files.sort(key=lambda x: get_frame_num(x['filename']))
            subdirs_data[subdir.name] = files

    if not subdirs_data:
        print("Ошибка: не найдено валидных данных", file=sys.stderr)
        return

    # Разделение на train/val
    subdirs = list(subdirs_data.keys())
    train_subdirs, val_subdirs = train_test_split(
        subdirs, train_size=args.train_ratio, random_state=42, shuffle=True
    )

    # Инициализация структур COCO
    coco_header = {
        "info": {"description": "Synthetic Smoke Dataset", "year": 2026},
        "licenses": [{"id": 1, "name": "Unknown"}],
        "categories": [{"id": 1, "name": "smoke", "supercategory": "none"}]
    }
    coco_train = {**coco_header, "images": [], "annotations": []}
    coco_val = {**coco_header, "images": [], "annotations": []}

    # Обработка выборок
    print("Обработка train...")
    for subdir in tqdm(train_subdirs):
        process_split(
            subdirs_data[subdir], coco_train,
            out / "train2017", out / "annotations", out / "original_annotations", args.n_prev
        )

    print("Обработка val...")
    for subdir in tqdm(val_subdirs):
        process_split(
            subdirs_data[subdir], coco_val,
            out / "val2017", out / "annotations", out / "original_annotations", args.n_prev
        )

    # Сохранение результатов
    with open(out / "annotations" / "instances_train2017.json", 'w', encoding='utf-8') as f:
        json.dump(coco_train, f, indent=2)
    with open(out / "annotations" / "instances_val2017.json", 'w', encoding='utf-8') as f:
        json.dump(coco_val, f, indent=2)

    print(f"\nГотово. Train: {len(coco_train['images'])} изобр., Val: {len(coco_val['images'])} изобр.")


if __name__ == "__main__":
    main()