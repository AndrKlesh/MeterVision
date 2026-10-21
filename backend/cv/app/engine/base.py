from abc import ABC, abstractmethod
from typing import List
from app.schemas import MeterReading


class OCREngine(ABC):
    @abstractmethod
    def recognize(self, image_bytes: bytes) -> List[MeterReading]:
        ...
