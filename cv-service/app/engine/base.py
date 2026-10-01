from abc import ABC, abstractmethod
from typing import List
from app.schemas import OCRResult

class OCREngine(ABC):
    @abstractmethod
    def recognize(self, image_bytes: bytes) -> List[OCRResult]:
        ...