from contextlib import asynccontextmanager

from fastapi import FastAPI

from app.api.routes import router
from app.dependencies import get_ocr_engine, get_preprocessor


@asynccontextmanager
async def lifespan(app: FastAPI):
    """
    Управляет жизненным циклом приложения: загружает тяжёлые зависимости
    (OCR-движок) один раз при старте и освобождает при остановке.
    """
    # Загружаем OCR-движок один раз (инициализация EasyOCR тяжёлая).
    app.state.ocr_engine = get_ocr_engine()
    app.state.preprocessor = get_preprocessor()

    yield

    # Освобождение ресурсов (если потребуется).
    app.state.ocr_engine = None
    app.state.preprocessor = None


app = FastAPI(
    title="MeterVision Vision Service",
    version="0.1.0",
    description="Сервис распознавания показаний счётчиков по фотографии.",
    lifespan=lifespan,
)

app.include_router(router)
