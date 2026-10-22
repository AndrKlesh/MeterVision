import json
import logging
import threading
import time
import uuid
from contextlib import asynccontextmanager
from typing import Optional

from fastapi import FastAPI, File, Header, Query, Request, UploadFile
from fastapi.responses import JSONResponse

from app.config import settings
from app.engine.factory import create_engine
from app.engine.mock_engine import SCENARIOS, MockOCREngine
from app.errors import (
    CvError,
    EmptyFileError,
    FileTooLargeError,
    InvalidImageError,
    NotReadyError,
    NotRecognizedError,
    UnknownMockScenarioError,
    UnsupportedMediaTypeError,
)
from app.schemas import ErrorBody, HealthResponse, OCRResponse

logging.basicConfig(level=settings.log_level, format="%(message)s")
logger = logging.getLogger("cv")


@asynccontextmanager
async def lifespan(app: FastAPI):
    # Прогрев в фоне: пока он идёт, сервер уже отвечает и /health отдаёт 503 not_ready
    threading.Thread(target=app.state.engine.warmup, name="warmup", daemon=True).start()
    yield


app = FastAPI(title="CV Service", lifespan=lifespan)
# Движок создаётся сразу (это дёшево), тяжёлая загрузка моделей идёт в warmup()
app.state.engine = create_engine(settings)

_ERROR_RESPONSES = {
    code: {"model": ErrorBody}
    for code in (400, 413, 415, 422, 500, 503)
}


@app.exception_handler(CvError)
async def cv_error_handler(request: Request, exc: CvError):
    return JSONResponse(status_code=exc.status_code, content={"detail": exc.to_detail()})


@app.exception_handler(Exception)
async def unexpected_error_handler(request: Request, exc: Exception):
    # Подробности только в логе, наружу не отдаём
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


# Объявлен как def: FastAPI запустит его в пуле потоков, и тяжёлое распознавание не блокирует /health
@app.post("/api/ocr", response_model=OCRResponse, responses=_ERROR_RESPONSES)
def recognize(
    request: Request,
    file: UploadFile = File(...),
    mock_scenario: Optional[str] = Query(None, description="Только при USE_MOCK=true, иначе игнорируется"),
    x_correlation_id: Optional[str] = Header(None),
):
    engine = request.app.state.engine
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

        if settings.use_mock and isinstance(engine, MockOCREngine):
            scenario = mock_scenario or "ok"
            if scenario not in SCENARIOS:
                raise UnknownMockScenarioError(
                    f"Неизвестный mock_scenario. Допустимо: {', '.join(SCENARIOS)}."
                )
            readings = engine.recognize(image_bytes, scenario=scenario)
        else:
            readings = engine.recognize(image_bytes)

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


@app.get("/health", response_model=HealthResponse, responses={503: {"model": ErrorBody}})
def health(request: Request):
    engine = request.app.state.engine
    if not engine.is_ready:
        raise NotReadyError()
    return HealthResponse(
        status="ok",
        mode="mock" if settings.use_mock else "real",
        engine_ready=True,
    )
