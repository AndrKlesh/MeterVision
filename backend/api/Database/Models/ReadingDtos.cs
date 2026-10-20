namespace MeterVision.Api.Models;

/// <summary>Результат распознавания фото счётчика.</summary>
public record RecognizeResponseDto(
    Guid ReadingId,
    decimal? Value,
    string? SerialNumber,
    double ValueConfidence,
    double SerialNumberConfidence
);

/// <summary>Данные, которые подтвердил или исправил пользователь.</summary>
public record ConfirmReadingDto(decimal Value, string SerialNumber);

/// <summary>Данные о показании для передачи клиенту.</summary>
public record ReadingDto(
    Guid Id,
    decimal Value,
    string SerialNumber,
    string Status,
    DateTime CreatedAt,
    DateTime? ConfirmedAt
);