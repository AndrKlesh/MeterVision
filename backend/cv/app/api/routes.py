from fastapi import APIRouter, Depends, File, HTTPException, UploadFile

from app.abstractions.image_preprocessor import ImagePreprocessor
from app.abstractions.ocr_engine import OCREngine
from app.dependencies import get_ocr_engine, get_preprocessor
from app.schemas.meter_reading import MeterReading
from app.services.recognition_service import RecognitionService

router = APIRouter(prefix="/api/v1", tags=["recognition"])


@router.post("/recognize", response_model=MeterReading)
async def recognize(
    file: UploadFile = File(...),
    preprocessor: ImagePreprocessor = Depends(get_preprocessor),
    ocr_engine: OCREngine = Depends(get_ocr_engine),
) -> MeterReading:
    if file.content_type not in ("image/jpeg", "image/png"):
        raise HTTPException(status_code=400, detail="Only JPEG and PNG are supported.")

    content = await file.read()
    if not content:
        raise HTTPException(status_code=400, detail="The file is empty.")

    service = RecognitionService(preprocessor, ocr_engine)
    try:
        return service.recognize(content)
    except ValueError as e:
        raise HTTPException(status_code=422, detail=str(e))
