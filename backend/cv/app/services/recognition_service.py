from app.abstractions.image_preprocessor import ImagePreprocessor
from app.abstractions.ocr_engine import OcrEngine
from app.schemas.meter_reading import MeterReading


class RecognitionService:
    """Оркестрация пайплайна: препроцессинг → OCR → постпроцессинг."""

    def __init__(
        self,
        preprocessor: ImagePreprocessor,
        ocr_engine: OcrEngine,
    ) -> None:
        # Зависимости внедряются снаружи — легко подменить моками в тестах.
        self._preprocessor = preprocessor
        self._ocr = ocr_engine

    def recognize(self, image_bytes: bytes) -> MeterReading:
        processed = self._preprocessor.process(image_bytes)
        texts = self._ocr.extract(processed)

        if not texts:
            raise ValueError("No text detected on the image.")

        serial, value, confidence = self._parse(texts)
        return MeterReading(
            serial_number=serial,
            reading_value=value,
            confidence=confidence,
        )
