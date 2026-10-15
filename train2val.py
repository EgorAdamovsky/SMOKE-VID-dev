#!/usr/bin/env python3
import argparse
import re
import shutil
import sys
from pathlib import Path

TRAIN_DIR = "train2017"
VAL_DIR = "val2017"
ANN_DIR = "annotations"

PATTERN = re.compile(r"train2017", re.IGNORECASE)
REPLACEMENT = "val2017"


def process_dataset(ds_path: Path) -> bool:
    train_dir = ds_path / TRAIN_DIR
    val_dir = ds_path / VAL_DIR
    ann_dir = ds_path / ANN_DIR

    if not train_dir.is_dir() or not any(train_dir.iterdir()):
        return False

    if val_dir.is_dir() and any(val_dir.iterdir()):
        return False

    print(f"Обработка: {ds_path.name}")
    
    val_dir.mkdir(parents=True, exist_ok=True)
    moved_count = 0
    renamed_count = 0

    for img_file in train_dir.iterdir():
        if img_file.is_file():
            target_path = val_dir / img_file.name
            if not target_path.exists():
                shutil.move(str(img_file), str(target_path))
                moved_count += 1

    if ann_dir.is_dir():
        for ann_file in ann_dir.iterdir():
            if ann_file.is_file() and PATTERN.search(ann_file.name):
                new_name = PATTERN.sub(REPLACEMENT, ann_file.name)
                target_ann = ann_dir / new_name
                if not target_ann.exists():
                    ann_file.rename(target_ann)
                    renamed_count += 1

    if moved_count > 0:
        try:
            if not any(train_dir.iterdir()):
                train_dir.rmdir()
        except OSError:
            pass

    print(f"  -> Перемещено изображений: {moved_count}, переименовано аннотаций: {renamed_count}")
    return True


def main():
    parser = argparse.ArgumentParser(description="Конвертация субдиректорий из TRAIN в VAL")
    parser.add_argument("target_dir", type=str, help="Путь к корневой директории с субдиректориями наборов данных")
    args = parser.parse_args()

    root = Path(args.target_dir).resolve()
    if not root.is_dir():
        print(f"Ошибка: директория не найдена или путь некорректен: {root}", file=sys.stderr)
        sys.exit(1)

    print(f"Поиск датасетов в: {root}")
    processed = 0
    
    for ds_path in root.iterdir():
        if ds_path.is_dir():
            if process_dataset(ds_path):
                processed += 1

    print(f"Готово. Обработано директорий: {processed}")


if __name__ == "__main__":
    main()