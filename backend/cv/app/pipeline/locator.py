"""Поиск дисплея. ЗАГЛУШКА (см. preprocessing.md, раздел 3)."""
from abc import ABC, abstractmethod
from typing import Any, Optional

from app.models import Box, Region, RegionKind


class RegionLocator(ABC):
    @abstractmethod
    def locate(self, image: Any) -> Optional[Region]:
        """Область дисплея или None, если не найден."""
        ...


class DisplayLocator(RegionLocator):
    def locate(self, image: Any) -> Optional[Region]:
        # Фиксированная область из образца (рабочие координаты)
        return Region(kind=RegionKind.DISPLAY, bbox=Box(100, 440, 680, 360), source="stub")
