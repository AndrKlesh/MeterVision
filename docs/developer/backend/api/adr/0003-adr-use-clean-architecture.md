# ADR-0003: Использование Clean Architecture 

## Статус
Принято

## Контекст
Проект развивается, и с ростом функциональности (распознавание показаний, аутентификация, работа с фото) стало заметно, что бизнес-логика перемешана с деталями реализации.

## Решение 

Использовать Clean Architecture с четырьмя слоями, размещёнными как папки в одном проекте MeterVision.Api.

| Слой | Папка | Содержимое | Зависит от |
|---|---|---|---|
| Domain | `Domain/` | Сущности, Value Objects, Enums, доменные исключения | Ни от чего |
| Application | `Application/` | Команды/запросы, Handler'ы, валидаторы (FluentValidation), Pipeline Behaviors, интерфейсы  | Domain |
| Infrastructure | `Infrastructure/` | Реализации репозиториев и сервисов | Application, Domain |
| Api | `Controllers/`, `Middleware/`, `Program.cs` | Контроллеры, middleware, DI-регистрация | Application, Infrastructure |
