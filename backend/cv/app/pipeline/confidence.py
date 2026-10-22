"""Оценка уверенности. Чистые функции. ЗАГЛУШКА: уверенность поля = уверенность токена
без штрафов (реальные штрафы: recognition.md, раздел 7)."""
from app.models import ParsedField


class ConfidenceCalculator:
    def field_confidence(self, field: ParsedField) -> float:
        return round(min(1.0, max(0.0, field.confidence)), 2)

    def overall(self, serial: float, value: float) -> float:
        return min(serial, value)
