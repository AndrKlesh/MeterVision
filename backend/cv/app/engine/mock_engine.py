import random
from app.engine.base import OCREngine
from app.schemas import OCRResult, BoundingBox

class MockOCREngine(OCREngine):
    def recognize(self, image_bytes: bytes) -> list[OCRResult]:
        return [
            OCRResult(
                text="Тестовый текст",
                confidence=round(random.uniform(0.85, 0.99), 2),
                bbox=BoundingBox(x=10, y=10, width=120, height=30),
            ),
            OCRResult(
                text="Invoice #12345",
                confidence=0.91,
                bbox=BoundingBox(x=10, y=50, width=150, height=25),
            ),
        ]