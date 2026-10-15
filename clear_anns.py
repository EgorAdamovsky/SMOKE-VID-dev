#!/usr/bin/env python3
import argparse
import json
import sys
from pathlib import Path


def clear_annotations(file_path: Path) -> int:
    """Очищает массив 'annotations' в JSON-файле формата COCO."""
    with open(file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)

    removed_count = len(data.get('annotations', []))
    data['annotations'] = []

    with open(file_path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)

    return removed_count


def main():
    parser = argparse.ArgumentParser(
        description="Очистка массива 'annotations' в JSON-файлах (для NEGATIVE выборок)"
    )
    parser.add_argument(
        "target_dirs", 
        nargs='+', 
        type=str, 
        help="Путь к одной или нескольким директориям с JSON-файлами"
    )
    args = parser.parse_args()

    total_files = 0
    total_removed = 0

    for dir_str in args.target_dirs:
        target_dir = Path(dir_str).resolve()
        if not target_dir.is_dir():
            print(f"Пропуск: директория не найдена: {target_dir}", file=sys.stderr)
            continue

        print(f"Обработка: {target_dir}")
        json_files = [f for f in target_dir.rglob("*.json") if f.is_file() and not f.name.endswith('.bak')]

        for json_file in json_files:
            try:
                removed = clear_annotations(json_file)
                total_files += 1
                total_removed += removed
            except Exception as e:
                print(f"  Ошибка в {json_file.name}: {e}", file=sys.stderr)

    print(f"Готово. Обработано файлов: {total_files}, всего удалено аннотаций: {total_removed}")


if __name__ == "__main__":
    main()