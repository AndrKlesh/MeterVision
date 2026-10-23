from app.config import Settings
from app.engine.base import OCREngine
from app.engine.easyocr_engine import EasyOCREngine
from app.engine.mock_engine import MockOCREngine
from app.pipeline.confidence import ConfidenceCalculator
from app.pipeline.locator import DisplayLocator
from app.pipeline.postprocessing import ReadingParser
from app.pipeline.preprocessing import Preprocessor
from app.pipeline.recognizer import EasyOcrRecognizer


def create_engine(settings: Settings) -> OCREngine:
    """Единственное место, где выбирается реализация движка."""
    if settings.use_mock:
        return MockOCREngine()
    return EasyOCREngine(
        settings=settings,
        preprocessor=Preprocessor(),
        locator=DisplayLocator(),
        recognizer=EasyOcrRecognizer(
            langs=settings.easyocr_langs,
            gpu=settings.easyocr_gpu,
            model_dir=settings.easyocr_model_dir,
            min_confidence=settings.min_token_confidence,
        ),
        parser=ReadingParser(),
        confidence=ConfidenceCalculator(),
    )
