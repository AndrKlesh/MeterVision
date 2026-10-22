from abc import ABC, abstractmethod
from typing import List
from app.schemas import MeterReading


class OCREngine(ABC):
    @abstractmethod
    def recognize(self, image_bytes: bytes) -> List[MeterReading]:
        """Бросает InvalidImageError и NotRecognizedError (app.errors)."""
        ...

    @property
    def is_ready(self) -> bool:
        """Готов ли движок принимать запросы (модели загружены и прогреты)."""
        return True

    def warmup(self) -> None:
        """Загрузка моделей и прогрев. Синхронная, вызывается в фоновом потоке при старте."""
        return None
