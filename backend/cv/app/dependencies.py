from functools import lru_cache

from app.abstractions.image_preprocessor import ImagePreprocessor
from app.abstractions.ocr_engine import OCREngine
from app.infrastructure.fake_ocr_engine import FakeOCREngine
from app.infrastructure.fake_preprocessor import FakeImagePreprocessor

@lru_cache
def get_ocr_engine() -> OCREngine:
    # Кэшируем — EasyOCR инициализируется один раз.
    return FakeOCREngine()


def get_preprocessor() -> ImagePreprocessor:
    return FakeImagePreprocessor()
