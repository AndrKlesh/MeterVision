from functools import lru_cache

from app.abstractions.image_preprocessor import ImagePreprocessor
from app.abstractions.ocr_engine import OcrEngine
from app.infrastructure.easyocr_engine import EasyOcrEngine
from app.infrastructure.opencv_preprocessor import OpenCvImagePreprocessor


@lru_cache
def get_ocr_engine() -> OcrEngine:
    # Кэшируем — EasyOCR инициализируется один раз.
    return EasyOcrEngine()


def get_preprocessor() -> ImagePreprocessor:
    return OpenCvImagePreprocessor()
