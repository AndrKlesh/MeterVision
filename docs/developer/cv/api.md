# API

## POST /api/ocr

```bash
curl -X POST http://localhost:8000/api/ocr \ -F "file=@photo.jpg"
```

**Ответ:**
```json
{
  "readings": [{
    "serial_number": "SN-00482913",
    "reading_value": "50.99",
    "confidence": 0.88,
    "bbox": {"x":10, "y":10, "width":180, "height":30}
  }]
}
```

**Коды ошибок:**

400 - Файл пустой или битый 
413 - Файл больше заданного лимита 
415 - Не JPEG/PNG 
422 - Ничего не распознали 

## GET /health

Проверка что сервис жив

```json
{"status": "ok", "mode": "mock"}
```

## Swagger UI

Живая документация с возможностью потыкать: `http://localhost:8000/docs`
