from pydantic import BaseModel, Field


class MeterReading(BaseModel):
    """Результат распознавания показаний счётчика."""

    serial_number: str = Field(
        ...,
        description="Серийный номер счётчика, распознанный с изображения.",
        examples=["ABC123456"],
    )
    reading_value: str = Field(
        ...,
        description="Показание счётчика (строка, чтобы сохранить ведущие нули).",
        examples=["00123.45"],
    )
    confidence: float = Field(
        ...,
        ge=0.0,
        le=1.0,
        description="Уверенность модели в результате, от 0 до 1.",
        examples=[0.87],
    )
    