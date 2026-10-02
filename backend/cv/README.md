# CV Service (заглушка)

REST API для распознавания показаний счётчиков на изображениях. Сейчас работает **в режиме заглушки**: картинка не обрабатывается, сервис возвращает заранее заготовленные данные (серийный номер и значение показания) в том же формате, в каком будет отвечать реальный сервис (OpenCV + EasyOCR). Это позволяет разрабатывать остальные части проекта (например, C#-клиент) независимо.

## Требования

- Python 3.10+
- Зависимости из `requirements.txt` (`fastapi`, `uvicorn`, `pydantic`, `python-multipart`)

## Установка

```bat
cd cv-service
python -m pip install -r requirements.txt
```

## Запуск

Вариант 1: двойной клик по `run_mock.bat` (лежит в корне проекта).

Вариант 2: вручную из папки `cv-service`:

```bat
python -m uvicorn app.main:app --reload
```

Сервис будет доступен на `http://127.0.0.1:8000`. Сам корень (`/`) маршрута не имеет и вернёт `404 Not Found` — это нормально, используйте конкретные адреса ниже (`/docs`, `/health`, `/api/ocr`).

Чтобы подключаться с других компьютеров в сети, добавьте `--host 0.0.0.0`:

```bat
python -m uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
```

Тогда адрес: `http://<IP_машины>:8000` (IP можно узнать командой `ipconfig`). При первом запуске разрешите доступ в брандмауэре Windows.

## Документация API

- Swagger UI: `http://127.0.0.1:8000/docs`
- OpenAPI-схема: `http://127.0.0.1:8000/openapi.json` (из неё можно сгенерировать клиент, например NSwag для C#)

## Эндпоинты

### `GET /health`

Проверка, что сервис запущен.

```json
{"status": "ok", "mode": "mock"}
```

### `POST /api/ocr`

Принимает изображение, возвращает показание счётчика (серийный номер и значение).

**Запрос:** `multipart/form-data`, поле `file` с изображением.

Пример через curl:

```bat
curl -X POST "http://127.0.0.1:8000/api/ocr" -F "file=@C:\path\to\image.jpg"
```

**Ответ:** `200 OK`

```json
{
  "readings": [
    {
      "serial_number": "SN-00482913",
      "reading_value": "00457.8",
      "confidence": 0.93,
      "bbox": { "x": 10, "y": 10, "width": 180, "height": 30 }
    }
  ]
}
```

| Поле | Тип | Описание |
|---|---|---|
| `readings` | массив | Найденные показания (сейчас всегда один элемент) |
| `serial_number` | string | Серийный номер счётчика |
| `reading_value` | string | Значение показания (строка, чтобы сохранять ведущие нули) |
| `confidence` | float (0..1) | Уверенность распознавания |
| `bbox.x`, `bbox.y` | int | Координаты левого верхнего угла блока, px |
| `bbox.width`, `bbox.height` | int | Размеры блока, px |

**Коды ответа:** `200` успех, `422` неверный формат запроса (например, нет поля `file`).

> Особенности заглушки: содержимое картинки игнорируется, ответ всегда содержит один элемент в `readings`; `confidence` каждый раз случайный (0.85–0.99).

## Пример вызова из C#

```csharp
using var client = new HttpClient();
using var content = new MultipartFormDataContent();
content.Add(new ByteArrayContent(imageBytes), "file", "scan.jpg");

var response = await client.PostAsync("http://127.0.0.1:8000/api/ocr", content);
response.EnsureSuccessStatusCode();
var json = await response.Content.ReadAsStringAsync();
```

DTO на стороне C# должны повторять структуру ответа (`readings`, `serial_number`, `reading_value`, `confidence`, `bbox`). Имена полей в JSON в нижнем регистре, при десериализации используйте `JsonSerializerOptions { PropertyNameCaseInsensitive = true }`.

## Структура проекта

```
cv-service/
├── app/
│   ├── main.py            # FastAPI-приложение и эндпоинты
│   ├── config.py          # настройки (USE_MOCK)
│   ├── schemas.py         # модели запроса/ответа (Pydantic)
│   └── engine/
│       ├── base.py        # интерфейс OCREngine
│       └── mock_engine.py # заглушка с фиктивными данными
├── requirements.txt
└── run_mock.bat           # запуск на Windows
```

## Дальнейшее развитие

Реальное распознавание (OpenCV + EasyOCR) пока не реализовано. Для его добавления достаточно написать второй класс, наследующий `OCREngine`, с тем же методом `recognize`. Формат ответа API при этом не изменится.
