"""Предобработка кадра. ЗАГЛУШКА: возвращает вход без изменений (см. preprocessing.md)."""
from dataclasses import dataclass
from typing import Any, List


@dataclass(frozen=True)
class CropVariant:
    name: str            # "contrast", "adaptive"...
    image: Any
    scale: float = 1.0   # во сколько раз вариант увеличен относительно кропа (нужно для пересчёта bbox)


class Preprocessor:
    def enhance(self, image: Any) -> Any:
        """Выравнивание контраста, подавление бликов на всём кадре."""
        return image

    def crop_variants(self, crop: Any) -> List[CropVariant]:
        """Несколько вариантов подготовленного кропа дисплея."""
        return [CropVariant(name="contrast", image=crop, scale=1.0)]
