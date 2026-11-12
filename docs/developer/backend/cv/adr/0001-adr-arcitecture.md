# ADR-0001: Слоистая архитектура vision-сервиса

## Статус

Принято

## Контекст

Сервис должен принимать изображение счётчика, выполнять препроцессинг (OpenCV), OCR (EasyOCR) и возвращать распознанные показания. Нужно, чтобы код был тестируемым (без реальных моделей в юнит-тестах) и заменяемым (OCR или препроцессор можно было поменять, не переписывая API).

Рассматривались альтернативы:
- **Монолит в одном файле** — быстро, но тесты требуют EasyOCR и OpenCV, замена компонента болезненна.
- **Clean Architecture с Domain** — избыточно для сервиса, который только трансформирует изображение в DTO.
- **Слоистая архитектура (api / services / abstractions / infrastructure)** — достаточно для разделения HTTP, оркестрации и технических деталей.

## Решение

**Использовать трёхслойную архитектуру с абстракциями через `Protocol`.**

| Слой | Папка | Содержимое |
|---|---|---|
| API | `api/` | Роуты FastAPI, валидация входного файла, HTTP-ошибки |
| Services | `services/` | `RecognitionService` — оркестрация пайплайна |
| Infrastructure | `infrastructure/` | `CVImagePreprocessor`, `EasyOCREngine` |
| Abstractions | `abstractions/` | `ImagePreprocessor`, `OCREngine` (Protocol) |
| Schemas | `schemas/` | `MeterReading` (Pydantic) |

**Правило зависимостей:** `api` -> `services` -> `abstractions` <- `infrastructure`. `services` не знает о конкретных реализациях, только об абстракциях.
