import json
import logging
import threading
import time
import uuid
from contextlib import asynccontextmanager
from typing import Optional

from fastapi import Depends, FastAPI, File, Header, Query, Request, UploadFile
from fastapi.responses import JSONResponse

from app import config
from app.config import Settings
from app.engine.base import OCREngine
from app.engine.factory import create_engine
from app.errors import (
    CvError,
    EmptyFileError,
    FileTooLargeError,
    InvalidImageError,
    NotReadyError,
    NotRecognizedError,
    UnsupportedMediaTypeError,
)
from app.schemas import ErrorBody, HealthResponse, OCRResponse

logger = logging.getLogger("cv")

_ERROR_RESPONSES = {code: {"model": ErrorBody} for code in (400, 413, 415, 422, 500, 503)}


# --- зависимости: единственное место, откуда эндпоинты берут движок и настройки ---
# В тестах подменяются через app.dependency_overrides[get_engine] / [get_settings]

def get_engine(request: Request) -> OCREngine:
    return request.app.state.engine


def get_settings(request: Request) -> Settings:
    return request.app.state.settings


# --- обработчики ошибок ---

async def cv_error_handler(request: Request, exc: CvError):
    return JSONResponse(status_code=exc.status_code, content={"detail": exc.to_detail()})


async def unexpected_error_handler(request: Request, exc: Exception):
    # Подробности только в логе
    logger.exception("unexpected error")
    return JSONResponse(
        status_code=500,
        content={"detail": {"code": "internal_error", "message": "Внутренняя ошибка сервиса."}},
    )


def _log_request(correlation_id: str, outcome: str, started: float, file_size: Optional[int]) -> None:
    # Не логируем содержимое изображений и распознанные значения (configuration.md, раздел 4)
    level = logging.INFO if outcome in ("recognized", "not_recognized") else logging.WARNING
    logger.log(level, json.dumps({
        "correlation_id": correlation_id,
        "outcome": outcome,
        "duration_ms": round((time.perf_counter() - started) * 1000),
        "file_size": file_size,
    }, ensure_ascii=False))


# --- эндпоинты ---

# Объявлен как def: FastAPI запустит его в пуле потоков, и тяжёлое распознавание не блокирует /health
def recognize(
    file: UploadFile = File(...),
    mock_scenario: Optional[str] = Query(None, description="Только при USE_MOCK=true, иначе игнорируется"),
    x_correlation_id: Optional[str] = Header(None),
    engine: OCREngine = Depends(get_engine),
    settings: Settings = Depends(get_settings),
):
    correlation_id = x_correlation_id or uuid.uuid4().hex
    started = time.perf_counter()
    file_size: Optional[int] = None
    outcome = "error"
    try:
        if not engine.is_ready:
            raise NotReadyError()

        if file.content_type not in settings.allowed_content_types:
            raise UnsupportedMediaTypeError(list(settings.allowed_content_types))

        # Читаем на 1 байт больше лимита, чтобы заметить превышение
        image_bytes = file.file.read(settings.max_file_size + 1)
        file_size = len(image_bytes)

        if file_size == 0:
            raise EmptyFileError()
        if file_size > settings.max_file_size:
            raise FileTooLargeError(settings.max_file_size_mb)

        # Заглушка выбирает сценарий, поэтому основной движок будет работать т.к. engine.for_request()
        readings = engine.for_request({"mock_scenario": mock_scenario}).recognize(image_bytes)

        outcome = "recognized"
        return OCRResponse(readings=readings)
    except NotRecognizedError:
        outcome = "not_recognized"
        raise
    except InvalidImageError:
        outcome = "invalid_image"
        raise
    except CvError as exc:
        outcome = exc.code
        raise
    finally:
        _log_request(correlation_id, outcome, started, file_size)


def health(
    engine: OCREngine = Depends(get_engine),
    settings: Settings = Depends(get_settings),
):
    if not engine.is_ready:
        raise NotReadyError()
    return HealthResponse(
        status="ok",
        mode="mock" if settings.use_mock else "real",
        engine_ready=True,
    )


def create_app(settings: Optional[Settings] = None, engine: Optional[OCREngine] = None) -> FastAPI:
    """Собирает приложение. Параметры нужны тестам: чтобы подменить движок и настройки, не меняя глобальный app."""
    settings = settings or config.settings
    engine = engine or create_engine(settings)

    logging.basicConfig(level=settings.log_level, format="%(message)s")

    @asynccontextmanager
    async def lifespan(app: FastAPI):
        # Прогрев в фоне: пока он идёт, сервер уже отвечает и /health отдаёт 503 not_ready
        threading.Thread(target=app.state.engine.warmup, name="warmup", daemon=True).start()
        yield

    app = FastAPI(title="CV Service", lifespan=lifespan)
    app.state.settings = settings
    app.state.engine = engine

    app.add_exception_handler(CvError, cv_error_handler)
    app.add_exception_handler(Exception, unexpected_error_handler)

    app.add_api_route("/api/ocr", recognize, methods=["POST"], response_model=OCRResponse,
                      responses=_ERROR_RESPONSES)
    app.add_api_route("/health", health, methods=["GET"], response_model=HealthResponse,
                      responses={503: {"model": ErrorBody}})
    return app


# Точка входа для `uvicorn app.main:app` (команда запуска остается прежней)
app = create_app()