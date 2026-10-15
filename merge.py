#!/usr/bin/env python3
import argparse
import json
import shutil
import sys
from pathlib import Path


def discover_datasets(root: Path) -> list:
    datasets = []
    for item in root.rglob("*"):
        if not item.is_dir():
            continue
        has_ann = (item / "annotations").is_dir()
        has_split = (item / "train2017").is_dir() or (item / "val2017").is_dir()
        if has_ann and has_split:
            datasets.append(item)
    datasets.sort()
    return datasets


def merge(ds_type: str, output_path: Path, input_paths: list, step: int, max_prev: int):
    out_img_dir = output_path / f"{ds_type}2017"
    out_ann_dir = output_path / "annotations"
    out_img_dir.mkdir(parents=True, exist_ok=True)
    out_ann_dir.mkdir(parents=True, exist_ok=True)

    all_series = []
    file_to_source = {}

    for ds_path in input_paths:
        ann_file = ds_path / "annotations" / f"instances_{ds_type}2017.json"
        img_dir = ds_path / f"{ds_type}2017"

        if not ann_file.exists():
            continue

        with open(ann_file, 'r', encoding='utf-8') as f:
            data = json.load(f)

        anns_by_id = {}
        for ann in data.get('annotations', []):
            anns_by_id.setdefault(ann['image_id'], []).append(ann)

        for img in data['images']:
            fname = img['file_name']
            all_series.append({
                'file_name': fname,
                'width': img['width'],
                'height': img['height'],
                'prev_imgs': img.get('prev_imgs', [])[-max_prev:],
                'annotations': anns_by_id.get(img['id'], []),
                'source_img_path': img_dir / fname
            })
            file_to_source[fname] = img_dir / fname
            for p in img.get('prev_imgs', []):
                file_to_source[p] = img_dir / p

    print(f"Всего серий из {len(input_paths)} датасетов: {len(all_series)}")

    selected = [all_series[i] for i in range(0, len(all_series), step)]
    print(f"После сэмплирования (step={step}): {len(selected)}")

    needed_files = set()
    for s in selected:
        needed_files.add(s['file_name'])
        needed_files.update(s['prev_imgs'])

    copied = 0
    for fname in needed_files:
        src = file_to_source.get(fname)
        if src and src.exists():
            shutil.copy2(src, out_img_dir / fname)
            copied += 1
        else:
            print(f"[!] Файл не найден: {fname}")
    print(f"Скопировано изображений: {copied}")

    new_images = []
    new_annotations = []
    fname_to_id = {}

    for s in selected:
        if s['file_name'] not in fname_to_id:
            new_id = len(new_images)
            new_images.append({
                'id': new_id,
                'file_name': s['file_name'],
                'width': s['width'],
                'height': s['height'],
                'prev_imgs': [p for p in s['prev_imgs'] if p in needed_files]
            })
            fname_to_id[s['file_name']] = new_id

        img_id = fname_to_id[s['file_name']]
        for ann in s['annotations']:
            new_annotations.append({
                'id': len(new_annotations),
                'image_id': img_id,
                'category_id': ann['category_id'],
                'bbox': ann['bbox'],
                'area': ann['area'],
                'segmentation': ann.get('segmentation', []),
                'iscrowd': ann.get('iscrowd', 0)
            })

    merged = {
        'info': {'description': 'Merged COCO Dataset', 'version': '1.0'},
        'licenses': [],
        'categories': [{'id': 1, 'name': 'smoke'}],
        'images': new_images,
        'annotations': new_annotations
    }

    out_ann_path = out_ann_dir / f"instances_{ds_type}2017.json"
    with open(out_ann_path, 'w', encoding='utf-8') as f:
        json.dump(merged, f, indent=2, ensure_ascii=False)

    print(f"Готово: {len(new_images)} изображений, {len(new_annotations)} аннотаций")
    print(f"Результат: {output_path}")


def main():
    parser = argparse.ArgumentParser(description="Сборка итогового COCO-датасета из субдиректорий")
    parser.add_argument("--source", type=str, required=True, help="Корневая директория с исходными датасетами")
    parser.add_argument("--output", type=str, required=True, help="Путь для сохранения результата")
    parser.add_argument("--type", type=str, required=True, choices=["train", "val"], help="Тип сплита (train или val)")
    parser.add_argument("--step", type=int, default=1, help="Шаг выборки серий (каждый N-й кадр)")
    parser.add_argument("--max-prev", type=int, default=10, help="Максимальная длина prev_imgs")
    args = parser.parse_args()

    source = Path(args.source).resolve()
    output = Path(args.output).resolve()

    if not source.is_dir():
        print(f"Ошибка: директория не найдена: {source}", file=sys.stderr)
        sys.exit(1)

    datasets = discover_datasets(source)
    print(f"Найдено валидных датасетов: {len(datasets)}")

    if not datasets:
        print("Датасеты не найдены.", file=sys.stderr)
        sys.exit(1)

    merge(args.type, output, datasets, args.step, args.max_prev)


if __name__ == "__main__":
    main()