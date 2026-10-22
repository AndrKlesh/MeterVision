"""Настройки сервиса из переменных окружения.

Старые имена констант (USE_MOCK, MAX_FILE_SIZE, MAX_FILE_SIZE_MB, ALLOWED_CONTENT_TYPES)
сохранены внизу файла для обратной совместимости.
Некорректное значение останавливает запуск понятной ошибкой ConfigError.
"""
import logging
import os
import re
from dataclasses import dataclass
from typing import Mapping, Optional, Tuple


class ConfigError(RuntimeError):
    """Некорректная настройка окружения."""


_TRUE = {"1", "true", "yes", "on"}
_FALSE = {"0", "false", "no", "off"}


def _raw(env: Mapping[str, str], name: str) -> Optional[str]:
    value = env.get(name)
    if value is None or value.strip() == "":
        return None
    return value.strip()


def _bool(env: Mapping[str, str], name: str, default: bool) -> bool:
    raw = _raw(env, name)
    if raw is None:
        return default
    if raw.lower() in _TRUE:
        return True
    if raw.lower() in _FALSE:
        return False
    raise ConfigError(f"{name}: ожидается true/false, получено {raw!r}")


def _int(env: Mapping[str, str], name: str, default: int, min_value: int = 0) -> int:
    raw = _raw(env, name)
    if raw is None:
        return default
    try:
        value = int(raw)
    except ValueError:
        raise ConfigError(f"{name}: ожидается целое число, получено {raw!r}") from None
    if value < min_value:
        raise ConfigError(f"{name}: значение должно быть не меньше {min_value}, получено {value}")
    return value


def _float(env: Mapping[str, str], name: str, default: float, lo: float, hi: float) -> float:
    raw = _raw(env, name)
    if raw is None:
        return default
    try:
        value = float(raw)
    except ValueError:
        raise ConfigError(f"{name}: ожидается число, получено {raw!r}") from None
    if not lo <= value <= hi:
        raise ConfigError(f"{name}: значение должно быть в диапазоне {lo}..{hi}, получено {value}")
    return value


def _csv(env: Mapping[str, str], name: str, default: str) -> Tuple[str, ...]:
    raw = _raw(env, name) or default
    items = tuple(part.strip() for part in raw.split(",") if part.strip())
    if not items:
        raise ConfigError(f"{name}: список не должен быть пустым")
    return items


@dataclass(frozen=True)
class Settings:
    use_mock: bool
    max_file_size_mb: int
    allowed_content_types: frozenset
    max_image_pixels: int
    max_side_px: int
    easyocr_langs: Tuple[str, ...]
    easyocr_gpu: bool
    easyocr_model_dir: str
    min_token_confidence: float
    value_decimals: int
    serial_pattern: str
    serial_min_digit_ratio: float
    max_concurrent_recognitions: int
    debug_save_dir: Optional[str]
    debug_return_raw_text: bool
    log_level: str

    @property
    def max_file_size(self) -> int:
        return self.max_file_size_mb * 1024 * 1024

    @property
    def value_pattern(self) -> str:
        """Формат значения: ^\\d{1,9}\\.\\d{N}$, N = VALUE_DECIMALS."""
        return rf"^\d{{1,9}}\.\d{{{self.value_decimals}}}$"


DEFAULT_SERIAL_PATTERN = r"^(?=.{6,20}$)[0-9A-Z]+(?:-[0-9A-Z]+)*$"


def load_settings(env: Optional[Mapping[str, str]] = None) -> Settings:
    env = os.environ if env is None else env

    serial_pattern = _raw(env, "SERIAL_PATTERN") or DEFAULT_SERIAL_PATTERN
    try:
        re.compile(serial_pattern)
    except re.error as exc:
        raise ConfigError(f"SERIAL_PATTERN: некорректное регулярное выражение ({exc})") from None

    log_level = (_raw(env, "LOG_LEVEL") or "INFO").upper()
    if not isinstance(logging.getLevelName(log_level), int):
        raise ConfigError(f"LOG_LEVEL: неизвестный уровень {log_level!r}")

    return Settings(
        use_mock=_bool(env, "USE_MOCK", True),
        max_file_size_mb=_int(env, "MAX_FILE_SIZE_MB", 10, min_value=1),
        allowed_content_types=frozenset(_csv(env, "ALLOWED_CONTENT_TYPES", "image/jpeg,image/png")),
        max_image_pixels=_int(env, "MAX_IMAGE_PIXELS", 40_000_000, min_value=1),
        max_side_px=_int(env, "MAX_SIDE_PX", 1600, min_value=64),
        easyocr_langs=_csv(env, "EASYOCR_LANGS", "en"),
        easyocr_gpu=_bool(env, "EASYOCR_GPU", False),
        easyocr_model_dir=_raw(env, "EASYOCR_MODEL_DIR") or "/models",
        min_token_confidence=_float(env, "MIN_TOKEN_CONFIDENCE", 0.2, 0.0, 1.0),
        value_decimals=_int(env, "VALUE_DECIMALS", 2, min_value=0),
        serial_pattern=serial_pattern,
        serial_min_digit_ratio=_float(env, "SERIAL_MIN_DIGIT_RATIO", 0.5, 0.0, 1.0),
        max_concurrent_recognitions=_int(env, "MAX_CONCURRENT_RECOGNITIONS", 1, min_value=1),
        debug_save_dir=_raw(env, "DEBUG_SAVE_DIR"),
        debug_return_raw_text=_bool(env, "DEBUG_RETURN_RAW_TEXT", False),
        log_level=log_level,
    )


settings = load_settings()

# --- обратная совместимость со старым config.py ---
USE_MOCK = settings.use_mock
MAX_FILE_SIZE_MB = settings.max_file_size_mb
MAX_FILE_SIZE = settings.max_file_size
ALLOWED_CONTENT_TYPES = set(settings.allowed_content_types)
