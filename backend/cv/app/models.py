"""Внутренние структуры конвейера. Не часть внешнего контракта (он в schemas.py)."""
from dataclasses import dataclass, field
from enum import Enum
from typing import Optional, Tuple


@dataclass(frozen=True)
class Box:
    """Прямоугольник в пикселях: x, y, width, height."""
    x: float
    y: float
    width: float
    height: float

    def scaled(self, factor: float) -> "Box":
        return Box(self.x * factor, self.y * factor, self.width * factor, self.height * factor)

    def shifted(self, dx: float, dy: float) -> "Box":
        return Box(self.x + dx, self.y + dy, self.width, self.height)

    def center(self) -> Tuple[float, float]:
        return self.x + self.width / 2, self.y + self.height / 2

    def contains_center_of(self, other: "Box") -> bool:
        cx, cy = other.center()
        return self.x <= cx <= self.x + self.width and self.y <= cy <= self.y + self.height


class RegionKind(str, Enum):
    DISPLAY = "display"
    FULL = "full"


@dataclass(frozen=True)
class Region:
    kind: RegionKind
    bbox: Box
    source: str  # способ нахождения: "contour", "stub", "fallback"...


@dataclass(frozen=True)
class Token:
    """Результат OCR. bbox в системе координат изображения, которое подали в recognizer."""
    text: str
    confidence: float
    bbox: Box
    rotation: int = 0
    variant: Optional[str] = None


@dataclass(frozen=True)
class ParsedField:
    value: str
    confidence: float
    bbox: Box
    # например: "substituted", "point_restored", "fallback_region"
    flags: Tuple[str, ...] = field(default_factory=tuple)
