# Алгоритмы

Работает заглушка `MockOCREngine` — возвращает тестовые данные, изображение не обрабатывает

```python
def recognize(self, image_bytes: bytes) -> list[MeterReading]:
        # Игнорируем содержимое картинки, возвращаем фиктивное, но правдоподобное показание — для разработки остальных частей проекта
        return [
            MeterReading(
                serial_number="SN-00482913",
                reading_value="50.99",
                confidence=round(random.uniform(0.85, 0.99), 2),
                bbox=BoundingBox(x=10, y=10, width=180, height=30),
            ),
        ]
```

## Интерфейс

`engine/base.py`:
```python
class OCREngine(ABC):
    @abstractmethod
    def recognize(self, image_bytes: bytes) -> List[MeterReading]:
        pass
```

На входе — байты изображения 
На выходе — список распознанных счётчиков

Каждый `MeterReading` содержит:
- `serial_number` - серийник
- `reading_value` - показания
- `confidence` - насколько уверены (0.0-1.0)
- `bbox` - область с данными 
