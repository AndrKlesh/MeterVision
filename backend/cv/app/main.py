from fastapi import FastAPI, UploadFile, File, HTTPException
from app.config import (
    USE_MOCK,
    MAX_FILE_SIZE,
    MAX_FILE_SIZE_MB,
    ALLOWED_CONTENT_TYPES,
)
from app.engine.mock_engine import MockOCREngine
from app.schemas import OCRResponse

app = FastAPI(title="CV Service (Mock)")

if USE_MOCK:
    engine = MockOCREngine()
else:
    raise RuntimeError("Реальный движок ещё не реализован. Установите USE_MOCK=true")


@app.post("/api/ocr", response_model=OCRResponse)
async def recognize(file: UploadFile = File(...)):
    if file.content_type not in ALLOWED_CONTENT_TYPES:
        raise HTTPException(
            status_code=415,
            detail=f"Неподдерживаемый тип файла. Допустимо: {', '.join(sorted(ALLOWED_CONTENT_TYPES))}",
        )

    # Читаем на 1 байт больше лимита, чтобы заметить превышение
    image_bytes = await file.read(MAX_FILE_SIZE + 1)

    if len(image_bytes) == 0:
        raise HTTPException(status_code=400, detail="Пустой файл")

    if len(image_bytes) > MAX_FILE_SIZE:
        raise HTTPException(
            status_code=413,
            detail=f"Файл слишком большой. Максимум {MAX_FILE_SIZE_MB} МБ",
        )

    readings = engine.recognize(image_bytes)
    return OCRResponse(readings=readings)


@app.get("/health")
def health():
    return {"status": "ok", "mode": "mock" if USE_MOCK else "real"}
