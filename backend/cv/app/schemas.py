from pydantic import BaseModel
from typing import List


class BoundingBox(BaseModel):
    x: int
    y: int
    width: int
    height: int


class MeterReading(BaseModel):
    serial_number: str
    reading_value: str
    confidence: float
    bbox: BoundingBox


class OCRResponse(BaseModel):
    readings: List[MeterReading]
