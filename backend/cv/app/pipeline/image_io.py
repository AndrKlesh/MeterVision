"""Декодирование и масштабирование. Единственная точка контакта с декодированием файла.

ЗАГЛУШКА: проверяет только сигнатуру JPEG/PNG, пиксели не декодирует.
Реальная версия: cv2.imdecode, EXIF-ориентация, проверка MAX_IMAGE_PIXELS до полного
декодирования, downscale до MAX_SIDE_PX (см. preprocessing.md, этапы 1-2).
"""
from dataclasses import dataclass
from typing import Any

from app.errors import InvalidImageError

_JPEG = b"\xff\xd8\xff"
_PNG = b"\x89PNG\r\n\x1a\n"


@dataclass(frozen=True)
class DecodedImage:
    pixels: Any          # рабочая копия (ndarray в реальной версии)
    width: int           # размеры рабочей копии
    height: int
    scale_to_original: float  # множитель: координаты рабочей копии -> исходное изображение с EXIF


def decode_image(image_bytes: bytes, max_pixels: int, max_side_px: int) -> DecodedImage:
    if not (image_bytes.startswith(_JPEG) or image_bytes.startswith(_PNG)):
        raise InvalidImageError()
    return DecodedImage(pixels=None, width=0, height=0, scale_to_original=1.0)


def crop(pixels: Any, bbox) -> Any:
    """Вырезает bbox (models.Box) из рабочей копии. ЗАГЛУШКА: возвращает вход как есть."""
    return pixels
