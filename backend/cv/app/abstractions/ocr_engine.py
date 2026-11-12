from typing import Protocol
from app.schemas.meter_reading import MeterReading
import numpy as np

class OCREngine(Protocol):
    """
    Контракт OCR-движка: извлекает показания счётчика с изображения.

    Реализации могут использовать разные библиотеки (EasyOCR, Tesseract,
    облачные API), но обязаны возвращать доменную модель ``MeterReading``.
    """

    def extract(self, image: np.ndarray) -> MeterReading:
        """
        Извлекает показания счётчика с подготовленного изображения.

        :param image: изображение после препроцессинга (numpy-массив).
        :return: распознанные показания в виде :class:`MeterReading`.
        :raises ValueError: если на изображении не удалось найти текст
            или распознать обязательные поля (серийный номер, показание).
        """
        ...
