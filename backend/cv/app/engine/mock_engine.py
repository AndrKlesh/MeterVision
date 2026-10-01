import random
from app.engine.base import OCREngine
from app.schemas import MeterReading, BoundingBox


class MockOCREngine(OCREngine):
    def recognize(self, image_bytes: bytes) -> list[MeterReading]:
        # Игнорируем содержимое картинки, возвращаем фиктивное, но правдоподобное показание — для разработки остальных частей проекта
        return [
            MeterReading(
                serial_number="SN-00482913",
                reading_value="00457.8",
                confidence=round(random.uniform(0.85, 0.99), 2),
                bbox=BoundingBox(x=10, y=10, width=180, height=30),
            ),
        ]
