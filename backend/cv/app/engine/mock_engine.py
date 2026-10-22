import random
import time
from typing import List

from app.engine.base import OCREngine
from app.errors import InvalidImageError, NotRecognizedError
from app.schemas import BoundingBox, MeterReading

SCENARIOS = ("ok", "low_confidence", "not_recognized", "invalid_image", "slow")


class MockOCREngine(OCREngine):
    """Заглушка для разработки backend и приложения. Содержимое картинки игнорируется.

    Сценарии (параметр scenario, в API это ?mock_scenario=...): см. docs/developer/api.md, раздел 7.
    """

    def __init__(self, slow_delay_s: float = 5.0):
        self._slow_delay_s = slow_delay_s

    def recognize(self, image_bytes: bytes, scenario: str = "ok") -> List[MeterReading]:
        if scenario == "not_recognized":
            raise NotRecognizedError(missing=["serial_number"])
        if scenario == "invalid_image":
            raise InvalidImageError()
        if scenario == "slow":
            time.sleep(self._slow_delay_s)

        low, high = (0.3, 0.6) if scenario == "low_confidence" else (0.85, 0.99)
        serial_conf = round(random.uniform(low, high), 2)
        value_conf = round(random.uniform(low, high), 2)
        return [
            MeterReading(
                serial_number="50794925145054",
                reading_value="50.99",
                confidence=min(serial_conf, value_conf),
                serial_confidence=serial_conf,
                value_confidence=value_conf,
                bbox=BoundingBox(x=100, y=440, width=680, height=360),
                serial_bbox=BoundingBox(x=5, y=390, width=40, height=450),
                raw_text=None,
            ),
        ]
