import numpy as np

from app.schemas.meter_reading import MeterReading


class FakeOCREngine:
    """
    Фейковая реализация ``OCREngine`` для тестов.

    Возвращает заранее заданный ``MeterReading`` без вызова EasyOCR.
    """

    def __init__(self, result: MeterReading | None = None) -> None:
        self._result = result or MeterReading(
            serial_number="ABC123456",
            reading_value="00123.45",
            confidence=0.95,
        )
        self.calls: list[np.ndarray] = []

    def extract(self, image: np.ndarray) -> MeterReading:
        self.calls.append(image)
        return self._result


class FailingOCREngine:
    """Фейковый OCR-движок, который всегда падает (имитация 'текста не найдено')."""

    def extract(self, image: np.ndarray) -> MeterReading:
        raise ValueError("No text detected on the image.")
