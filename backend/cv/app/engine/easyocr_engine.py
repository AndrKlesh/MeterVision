"""Оркестратор конвейера (architecture.md, раздел 5). Зависимости передаются через конструктор."""
import dataclasses
import logging
import threading
from typing import List, Optional

from app import config
from app.engine.base import OCREngine
from app.errors import NotRecognizedError
from app.models import Box, ParsedField, Region, RegionKind, Token
from app.pipeline import image_io
from app.pipeline.confidence import ConfidenceCalculator
from app.pipeline.locator import RegionLocator
from app.pipeline.postprocessing import ReadingParser
from app.pipeline.preprocessing import Preprocessor
from app.pipeline.recognizer import TextRecognizer
from app.schemas import BoundingBox, MeterReading

logger = logging.getLogger("cv.engine")

VALUE_ALLOWLIST = "0123456789."
SERIAL_ALLOWLIST = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-"
SERIAL_ROTATIONS = (0, 90, 270)


class EasyOCREngine(OCREngine):
    def __init__(
        self,
        settings: config.Settings,
        preprocessor: Preprocessor,
        locator: RegionLocator,
        recognizer: TextRecognizer,
        parser: ReadingParser,
        confidence: ConfidenceCalculator,
    ):
        self._settings = settings
        self._pre = preprocessor
        self._locator = locator
        self._recognizer = recognizer
        self._parser = parser
        self._confidence = confidence
        self._ready = threading.Event()
        self._slots = threading.BoundedSemaphore(settings.max_concurrent_recognitions)

    # --- готовность ---
    @property
    def is_ready(self) -> bool:
        return self._ready.is_set()

    def warmup(self) -> None:
        self._recognizer.load()
        self._ready.set()

    # --- основной метод ---
    def recognize(self, image_bytes: bytes) -> List[MeterReading]:
        with self._slots:
            return [self._recognize(image_bytes)]

    def _recognize(self, image_bytes: bytes) -> MeterReading:
        s = self._settings
        # 1. декодирование (InvalidImageError)
        image = image_io.decode_image(image_bytes, s.max_image_pixels, s.max_side_px)
        # 2. предобработка кадра
        frame = self._pre.enhance(image.pixels)
        # 3. дисплей; если не найден, значение ищем по всему кадру
        region = self._locator.locate(frame)
        fallback = region is None
        if fallback:
            region = Region(RegionKind.FULL, Box(0, 0, image.width, image.height), "fallback")
        # 4. значение: по каждому варианту кропа, лучший по уверенности
        value, value_tokens = self._read_value(frame, region)
        if value is not None and fallback:
            value = dataclasses.replace(value, flags=value.flags + ("fallback_region",))
        # 5. серийный номер: полный кадр, токены внутри дисплея исключаются
        serial_tokens = self._recognizer.read(frame, SERIAL_ALLOWLIST, SERIAL_ROTATIONS)
        if region.kind is RegionKind.DISPLAY:
            serial_tokens = [t for t in serial_tokens if not region.bbox.contains_center_of(t.bbox)]
        serial = self._parser.parse_serial(serial_tokens)
        # 7. нет поля -> not_recognized (штатный ответ)
        missing = [name for name, f in (("reading_value", value), ("serial_number", serial)) if f is None]
        if missing:
            raise NotRecognizedError(missing=missing)
        # 8. уверенность, bbox в координаты исходного изображения, сборка ответа
        serial_conf = self._confidence.field_confidence(serial)
        value_conf = self._confidence.field_confidence(value)
        raw_text = None
        if s.debug_return_raw_text:
            raw_text = " ".join(t.text for t in value_tokens + serial_tokens)
        return MeterReading(
            serial_number=serial.value,
            reading_value=value.value,
            confidence=self._confidence.overall(serial_conf, value_conf),
            serial_confidence=serial_conf,
            value_confidence=value_conf,
            bbox=self._to_bbox(value.bbox, image.scale_to_original),
            serial_bbox=self._to_bbox(serial.bbox, image.scale_to_original),
            raw_text=raw_text,
        )

    def _read_value(self, frame, region: Region):
        crop = image_io.crop(frame, region.bbox)
        best: Optional[ParsedField] = None
        all_tokens: List[Token] = []
        for variant in self._pre.crop_variants(crop):
            raw = self._recognizer.read(variant.image, VALUE_ALLOWLIST, (0,))
            # координаты варианта -> координаты кадра: убрать увеличение, сдвинуть на начало кропа
            tokens = [
                dataclasses.replace(
                    t,
                    bbox=t.bbox.scaled(1 / variant.scale).shifted(region.bbox.x, region.bbox.y),
                    variant=variant.name,
                )
                for t in raw
            ]
            all_tokens.extend(tokens)
            field = self._parser.parse_value(tokens)
            if field is not None and (best is None or field.confidence > best.confidence):
                best = field
        return best, all_tokens

    @staticmethod
    def _to_bbox(box: Box, scale: float) -> BoundingBox:
        b = box.scaled(scale)
        return BoundingBox(x=round(b.x), y=round(b.y), width=round(b.width), height=round(b.height))
