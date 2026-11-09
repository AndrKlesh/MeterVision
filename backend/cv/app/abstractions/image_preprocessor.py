from typing import Protocol

import numpy as np


class ImagePreprocessor(Protocol):
    """Контракт препроцессора: готовит изображение к OCR."""

    def process(self, image_bytes: bytes) -> np.ndarray:
        """
        Преобразует байты изображения в обработанный numpy-массив.

        :param image_bytes: сырые байты файла (JPEG/PNG).
        :return: обработанное изображение.
        :raises ValueError: если изображение не удалось декодировать.
        """
        ...
