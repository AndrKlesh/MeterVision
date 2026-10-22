from pydantic import BaseModel, Field
from typing import List, Optional


class BoundingBox(BaseModel):
    x: int
    y: int
    width: int
    height: int


class MeterReading(BaseModel):
    serial_number: str
    reading_value: str
    # Общая уверенность: минимум из двух полей (оставлена для обратной совместимости)
    confidence: float = Field(ge=0, le=1)
    serial_confidence: float = Field(ge=0, le=1)
    value_confidence: float = Field(ge=0, le=1)
    # Область значения
    bbox: BoundingBox
    # Область серийного номера
    serial_bbox: BoundingBox
    # Весь распознанный текст; только при DEBUG_RETURN_RAW_TEXT=true
    raw_text: Optional[str] = None


class OCRResponse(BaseModel):
    readings: List[MeterReading]


class ErrorDetail(BaseModel):
    code: str
    message: str
    missing: Optional[List[str]] = None


class ErrorBody(BaseModel):
    detail: ErrorDetail


class HealthResponse(BaseModel):
    status: str
    mode: str
    engine_ready: bool
