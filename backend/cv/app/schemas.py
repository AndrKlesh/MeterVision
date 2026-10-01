from pydantic import BaseModel

class BoundingBox(BaseModel):
    x: int
    y: int
    width: int
    height: int

class OCRResult(BaseModel):
    text: str
    confidence: float
    bbox: BoundingBox

class OCRResponse(BaseModel):
    results: list[OCRResult]