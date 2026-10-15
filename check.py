#!/usr/bin/env python3
import argparse
import sys
import cv2
import numpy as np
from pathlib import Path
from pycocotools.coco import COCO


def main():
    parser = argparse.ArgumentParser(description="Визуализация серий кадров из датасета COCO")
    parser.add_argument("dataset_dir", type=str, help="Путь к корню датасета")
    parser.add_argument("--max-series", type=int, default=50, help="Максимальное количество серий")
    parser.add_argument("--max-prev", type=int, default=5, help="Количество предыдущих кадров в серии")
    args = parser.parse_args()

    root = Path(args.dataset_dir).resolve()
    if not root.is_dir():
        print(f"Ошибка: директория не найдена: {root}", file=sys.stderr)
        sys.exit(1)

    ann_dir = root / "annotations"
    if not ann_dir.is_dir():
        print(f"Ошибка: директория annotations не найдена: {ann_dir}", file=sys.stderr)
        sys.exit(1)

    json_files = [f for f in ann_dir.iterdir() if f.is_file() and f.suffix.lower() == ".json"]
    if not json_files:
        print(f"Ошибка: JSON-файлы не найдены в {ann_dir}", file=sys.stderr)
        sys.exit(1)

    ann_path = json_files[0]
    if len(json_files) > 1:
        print(f"[!] Найдено {len(json_files)} JSON-файлов, используется: {ann_path.name}")

    coco = COCO(str(ann_path))
    img_ids = coco.getImgIds()
    if not img_ids:
        print("Ошибка: в аннотациях нет изображений", file=sys.stderr)
        sys.exit(1)

    # Определяем директорию изображений по первому файлу из JSON
    first_img = coco.loadImgs(img_ids[0])[0]['file_name']
    img_path = next(root.rglob(first_img), None)
    if img_path is None:
        print(f"Ошибка: не удалось найти изображение {first_img} внутри {root}", file=sys.stderr)
        sys.exit(1)
    img_dir = img_path.parent
    print(f"[i] Аннотации: {ann_path.name}")
    print(f"[i] Изображения: {img_dir}")

    out_dir = root / "visualizations"
    out_dir.mkdir(parents=True, exist_ok=True)

    count = 0
    for img_id in img_ids:
        if count >= args.max_series:
            break

        img_info = coco.loadImgs(img_id)[0]
        main_path = img_dir / img_info['file_name']

        if not main_path.exists():
            continue

        main_img = cv2.imread(str(main_path))
        if main_img is None:
            continue

        h, w = main_img.shape[:2]

        prev_files = img_info.get('prev_imgs', [])[-args.max_prev:] if args.max_prev > 0 else []
        prev_imgs = []
        for pf in prev_files:
            p_path = img_dir / pf
            if p_path.exists():
                p_img = cv2.imread(str(p_path))
                if p_img is not None:
                    prev_imgs.append(cv2.resize(p_img, (w, h)))

        all_frames = prev_imgs + [main_img]
        mosaic = np.hstack(all_frames)

        ann_ids = coco.getAnnIds(imgIds=img_id)
        if ann_ids:
            offset_x = (len(all_frames) - 1) * w
            for ann in coco.loadAnns(ann_ids):
                if 'bbox' in ann:
                    x, y, bw, bh = map(int, ann['bbox'])
                    x += offset_x
                    cv2.rectangle(mosaic, (x, y), (x + bw, y + bh), (0, 0, 255), 2)
                    cv2.putText(mosaic, "smoke", (x, y - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 1)

        label = "smoke" if ann_ids else "no_smoke"
        cv2.imwrite(str(out_dir / f"series_{img_id:06d}_{label}.jpg"), mosaic)
        count += 1

    print(f"Визуализировано серий: {count} (сохранено в {out_dir})")


if __name__ == "__main__":
    main()