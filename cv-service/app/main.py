from fastapi import FastAPI, UploadFile, File
from app.config import USE_MOCK
from app.engine.mock_engine import MockOCREngine
from app.schemas import OCRResponse

app = FastAPI(title="CV Service (Mock)")

if USE_MOCK:
    engine = MockOCREngine()
else:
    raise RuntimeError("Реальный движок ещё не реализован. Установите USE_MOCK=true")

@app.post("/api/ocr", response_model=OCRResponse)
async def recognize(file: UploadFile = File(...)):
    image_bytes = await file.read()
    results = engine.recognize(image_bytes)
    return OCRResponse(results=results)

@app.get("/health")
def health():
    return {"status": "ok", "mode": "mock" if USE_MOCK else "real"}