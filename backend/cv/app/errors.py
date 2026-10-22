"""Ошибки сервиса. Каждая знает свой HTTP-статус и код (см. docs/developer/api.md, раздел 4)."""
from typing import Any, Dict, List, Optional


class CvError(Exception):
    status_code = 500
    code = "internal_error"
    default_message = "Внутренняя ошибка сервиса."

    def __init__(self, message: Optional[str] = None):
        self.message = message or self.default_message
        super().__init__(self.message)

    def to_detail(self) -> Dict[str, Any]:
        return {"code": self.code, "message": self.message}


class EmptyFileError(CvError):
    status_code = 400
    code = "empty_file"
    default_message = "Пустой файл."


class InvalidImageError(CvError):
    """Файл не декодируется как изображение или слишком велик по числу пикселей."""
    status_code = 400
    code = "invalid_image"
    default_message = "Файл не удалось прочитать как изображение."


class FileTooLargeError(CvError):
    status_code = 413
    code = "file_too_large"

    def __init__(self, max_mb: int):
        super().__init__(f"Файл слишком большой. Максимум {max_mb} МБ.")


class UnsupportedMediaTypeError(CvError):
    status_code = 415
    code = "unsupported_media_type"

    def __init__(self, allowed: List[str]):
        super().__init__(f"Неподдерживаемый тип файла. Допустимо: {', '.join(sorted(allowed))}.")


class NotRecognizedError(CvError):
    """Штатный ответ, не сбой: не найдено значение и/или серийный номер."""
    status_code = 422
    code = "not_recognized"
    default_message = (
        "Не удалось распознать данные. Сфотографируйте счётчик так, "
        "чтобы в кадре были экран и серийный номер, без бликов."
    )

    def __init__(self, missing: List[str]):
        self.missing = list(missing)
        super().__init__()

    def to_detail(self) -> Dict[str, Any]:
        detail = super().to_detail()
        detail["missing"] = self.missing
        return detail


class NotReadyError(CvError):
    status_code = 503
    code = "not_ready"
    default_message = "Сервис ещё загружает модели."


class UnknownMockScenarioError(CvError):
    """Только для заглушки: неизвестное значение mock_scenario."""
    status_code = 400
    code = "unknown_mock_scenario"
