"""Выбор кандидатов и нормализация. Чистые функции без OpenCV/EasyOCR.

ЗАГЛУШКА: берёт первый подходящий по самым простым признакам токен, без чистки и проверки формата
(реальные правила: recognition.md, раздел 5).
"""
from typing import List, Optional

from app.models import ParsedField, Token


def _best(tokens: List[Token]) -> Optional[Token]:
    return max(tokens, key=lambda t: t.confidence, default=None)


class ReadingParser:
    def parse_value(self, tokens: List[Token]) -> Optional[ParsedField]:
        best = _best([t for t in tokens if "." in t.text])
        if best is None:
            return None
        return ParsedField(best.text, best.confidence, best.bbox, flags=("stub",))

    def parse_serial(self, tokens: List[Token]) -> Optional[ParsedField]:
        best = _best([t for t in tokens if "." not in t.text and len(t.text) >= 6])
        if best is None:
            return None
        return ParsedField(best.text, best.confidence, best.bbox, flags=("stub",))
