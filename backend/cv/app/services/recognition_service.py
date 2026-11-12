import re

from app.abstractions.image_preprocessor import ImagePreprocessor
from app.abstractions.ocr_engine import OCREngine
from app.schemas.meter_reading import MeterReading

class RecognitionService:
    """Оркестрация пайплайна: препроцессинг → OCR → постпроцессинг."""

    def __init__(
        self,
        preprocessor: ImagePreprocessor,
        ocr_engine: OCREngine,
    ) -> None:
        self._preprocessor = preprocessor
        self._ocr = ocr_engine

    def recognize(self, image_bytes: bytes) -> MeterReading:
        """
        Распознаёт показания счётчика по фотографии.

        :param image_bytes: сырые байты изображения (JPEG/PNG).
        :return: распознанные показания.
        :raises ValueError: если изображение не декодируется или текст не найден.
        """
        processed = self._preprocessor.process(image_bytes)
        return self._ocr.extract(processed)
