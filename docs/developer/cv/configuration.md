## Запустить

Через батник:
run_mock.bat

Или вручную:
```bash
cd backend/cv
export USE_MOCK=true
export PORT=8000
python -m uvicorn app.main:app --reload --port %PORT%
```

Запустится на `http://localhost:8000`, api документация `/docs`

## USE_MOCK

По умолчанию `true` — возвращает тестовые данные
Если поставить `false`, сервис упадёт при старте с ошибкой - реальный движок ещё не готов

## API_PORT

Порт по умолчанию `8000`

## MAX_FILE_SIZE_MB

Лимит на размер файла 10 МБ в `config.py`

Файлы больше лимита отклоняются с ошибкой 413

## Зависимости

Все версии пакетов в `requirements.txt`
