"""Обёртка над EasyOCR. Единственная точка контакта с EasyOCR.

ЗАГЛУШКА: easyocr не импортируется, возвращаются фиксированные токены образца.
Реальная версия создаёт easyocr.Reader один раз в load() и защищает доступ к нему
блокировкой (потокобезопасность Reader не гарантируется).
"""
import logging
from abc import ABC, abstractmethod
from typing import Any, List, Sequence

from app.models import Box, Token

logger = logging.getLogger("cv.recognizer")


class TextRecognizer(ABC):
    def load(self) -> None:
        """Загрузка моделей. Вызывается один раз при прогреве."""
        return None

    @abstractmethod
    def read(self, image: Any, allowlist: str, rotations: Sequence[int] = (0,)) -> List[Token]:
        """Токены; bbox в координатах переданного изображения."""
        ...


class EasyOcrRecognizer(TextRecognizer):
    def __init__(self, langs: Sequence[str], gpu: bool, model_dir: str, min_confidence: float):
        self._langs = tuple(langs)
        self._gpu = gpu
        self._model_dir = model_dir
        self._min_confidence = min_confidence

    def load(self) -> None:
        logger.warning("EasyOcrRecognizer — ЗАГЛУШКА: модели не загружаются, ответы фиктивные")

    def read(self, image: Any, allowlist: str, rotations: Sequence[int] = (0,)) -> List[Token]:
        if "." in allowlist:
            # значение: координаты относительно кропа дисплея
            return [Token("50.99", 0.94, Box(0, 0, 680, 360))]
        # серийный номер: координаты полного кадра, повёрнут на 90°
        return [Token("50794925145054", 0.88, Box(5, 390, 40, 450), rotation=90)]
