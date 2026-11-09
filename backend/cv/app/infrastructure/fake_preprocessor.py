import numpy as np


class FakeImagePreprocessor:
    """
    Фейковый препроцессор: возвращает пустой массив заданного размера,
    не вызывая OpenCV.
    """

    def __init__(self, shape: tuple[int, int] = (10, 10)) -> None:
        self._shape = shape
        self.calls: list[bytes] = []

    def process(self, image_bytes: bytes) -> np.ndarray:
        self.calls.append(image_bytes)
        return np.zeros(self._shape, dtype=np.uint8)


class FailingImagePreprocessor:
    """Фейковый препроцессор, который всегда падает (имитация 'битого изображения')."""

    def process(self, image_bytes: bytes) -> np.ndarray:
        raise ValueError("Could not decode the image.")
