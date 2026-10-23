from abc import ABC, abstractmethod
from typing import List, Mapping, Optional
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

    def for_request(self, options: Mapping[str, Optional[str]]) -> "OCREngine":
        """Хук для параметров конкретного запроса (сейчас: mock_scenario).

        По умолчанию параметры игнорируются и возвращается сам движок, поэтому
        основной движок не знает о заглушке, а эндпоинту не нужны проверки типа.
        """
        return self