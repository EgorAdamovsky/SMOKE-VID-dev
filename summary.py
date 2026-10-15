#!/usr/bin/env python3
import argparse
import json
import sys
from collections import defaultdict
from pathlib import Path

IMAGE_EXTS = {".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".webp"}
TRAIN_DIR = "train2017"
VAL_DIR = "val2017"
ANN_DIR = "annotations"


def get_source(path: Path) -> str:
    parts = [p.upper() for p in path.parts]
    if "SYNTH" in parts:
        return "synth"
    if "REAL" in parts:
        return "real"
    return "unknown"


def get_images_in_dir(dir_path: Path) -> set:
    if not dir_path.is_dir():
        return set()
    return {
        f.name for f in dir_path.rglob("*") 
        if f.is_file() and f.suffix.lower() in IMAGE_EXTS
    }


def analyze_split(img_dir: Path, ann_dir: Path, split_name: str) -> dict:
    res = {
        "total": 0, "pos": 0, "neg": 0,
        "missing_imgs": [], "extra_imgs": [], "semantic_errors": []
    }
    
    fs_imgs = get_images_in_dir(img_dir)
    res["total"] = len(fs_imgs)
    if not fs_imgs:
        return res

    ann_files = [
        f for f in ann_dir.iterdir() 
        if f.is_file() and f.suffix.lower() == ".json" and split_name.lower() in f.stem.lower()
    ]
    if not ann_files:
        return res

    with open(ann_files[0], 'r', encoding='utf-8') as f:
        data = json.load(f)

    json_imgs = {
        img["file_name"]: img["id"] 
        for img in data.get("images", []) 
        if "file_name" in img and "id" in img
    }
    
    ann_counts = defaultdict(int)
    for ann in data.get("annotations", []):
        ann_counts[ann.get("image_id")] += 1

    res["missing_imgs"] = sorted(set(json_imgs.keys()) - fs_imgs)
    res["extra_imgs"] = sorted(fs_imgs - set(json_imgs.keys()))

    # Проверка POS/NEG семантики
    for img_name, img_id in json_imgs.items():
        img_path = next((p for p in img_dir.rglob(img_name) if p.is_file()), None)
        if not img_path:
            continue

        ann_count = ann_counts.get(img_id, 0)
        parts = [p.upper() for p in img_path.parts]

        if "POSITIVE" in parts:
            res["pos"] += 1
            if ann_count == 0:
                res["semantic_errors"].append(f"POS без аннотаций: {img_name}")
        elif "NEGATIVE" in parts:
            res["neg"] += 1
            if ann_count > 0:
                res["semantic_errors"].append(f"NEG с аннотациями: {img_name}")

    return res


def analyze_dataset(ds_path: Path) -> dict:
    report = {
        "name": ds_path.name,
        "source": get_source(ds_path),
        "train": analyze_split(ds_path / TRAIN_DIR, ds_path / ANN_DIR, "train"),
        "val": analyze_split(ds_path / VAL_DIR, ds_path / ANN_DIR, "val"),
        "errors": []
    }

    if report["train"]["missing_imgs"] or report["train"]["extra_imgs"]:
        report["errors"].append(f"Train mismatch: miss={len(report['train']['missing_imgs'])}, extra={len(report['train']['extra_imgs'])}")
    if report["val"]["missing_imgs"] or report["val"]["extra_imgs"]:
        report["errors"].append(f"Val mismatch: miss={len(report['val']['missing_imgs'])}, extra={len(report['val']['extra_imgs'])}")
    
    report["errors"].extend(report["train"]["semantic_errors"])
    report["errors"].extend(report["val"]["semantic_errors"])

    if report["train"]["total"] == 0 and report["val"]["total"] == 0:
        report["errors"].append("Пустой датасет (нет изображений)")

    return report


def main():
    parser = argparse.ArgumentParser(description="Анализ и валидация микро-датасетов в формате COCO")
    parser.add_argument("root_dir", type=str, help="Корневая директория для рекурсивного поиска датасетов")
    parser.add_argument("--detailed", action="store_true", help="Выводить детальный список ошибок по каждому датасету")
    args = parser.parse_args()

    root = Path(args.root_dir).resolve()
    if not root.is_dir():
        print(f"Ошибка: директория не найдена: {root}", file=sys.stderr)
        sys.exit(1)

    # ИСПРАВЛЕНО: Рекурсивный поиск директорий, содержащих хотя бы одну из целевых папок
    datasets = [
        p for p in root.rglob("*") 
        if p.is_dir() and any((p / d).is_dir() for d in [TRAIN_DIR, VAL_DIR, ANN_DIR])
    ]
    
    if not datasets:
        print(f"Датасеты не найдены в: {root}")
        print("Убедитесь, что внутри есть папки 'train2017', 'val2017' или 'annotations'.")
        sys.exit(0)

    print(f"Найдено датасетов: {len(datasets)}\n")
    reports = [analyze_dataset(ds) for ds in datasets]
    
    # Агрегация статистики
    stats = {
        "total_ds": len(reports),
        "ds_with_errors": sum(1 for r in reports if r["errors"]),
        "train_tot": sum(r["train"]["total"] for r in reports),
        "train_pos": sum(r["train"]["pos"] for r in reports),
        "train_neg": sum(r["train"]["neg"] for r in reports),
        "val_tot": sum(r["val"]["total"] for r in reports),
        "val_pos": sum(r["val"]["pos"] for r in reports),
        "val_neg": sum(r["val"]["neg"] for r in reports),
        "src_real": sum(1 for r in reports if r["source"] == "real"),
        "src_synth": sum(1 for r in reports if r["source"] == "synth"),
        "src_unknown": sum(1 for r in reports if r["source"] == "unknown"),
    }

    # Вывод таблицы
    print(f"{'ДАТАСЕТ':<40} | {'SRC':<6} | {'TRAIN (Tot/Pos/Neg)':<20} | {'VAL (Tot/Pos/Neg)':<20} | {'СТАТУС'}")
    print("-" * 115)
    for r in sorted(reports, key=lambda x: x["name"]):
        t_str = f"{r['train']['total']}/{r['train']['pos']}/{r['train']['neg']}"
        v_str = f"{r['val']['total']}/{r['val']['pos']}/{r['val']['neg']}"
        status = f"ОШИБКИ: {len(r['errors'])}" if r["errors"] else "OK"
        print(f"{r['name']:<40} | {r['source']:<6} | {t_str:<20} | {v_str:<20} | {status}")
        
        if args.detailed and r["errors"]:
            for err in r["errors"]:
                print(f"    -> {err}")

    print("-" * 115)
    print(f"\nИТОГО:")
    print(f"  Датасетов: {stats['total_ds']} (с ошибками: {stats['ds_with_errors']})")
    print(f"  Источники сцен: REAL={stats['src_real']}, SYNTH={stats['src_synth']}, UNKNOWN={stats['src_unknown']}")
    print(f"  TRAIN: всего={stats['train_tot']}, POS={stats['train_pos']}, NEG={stats['train_neg']}")
    print(f"  VAL:   всего={stats['val_tot']}, POS={stats['val_pos']}, NEG={stats['val_neg']}")


if __name__ == "__main__":
    main()