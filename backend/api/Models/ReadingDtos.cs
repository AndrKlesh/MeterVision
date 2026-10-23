namespace MeterVision.Api.Models;

/// <summary>Результат распознавания фото счётчика.</summary>
public record RecognizeResponseDto(
    int Id,
    double? Value,
    string? SerialNumber,
    double Confidence);

/// <summary>Данные, которые подтвердил или исправил пользователь.</summary>
public record ConfirmReadingDto(double Value, string SerialNumber);

/// <summary>Счётчик с последним показанием.</summary>
public record MeterDetailDto(
    int Id,
    string SerialNumber,
    string Type,
    string Status,
    double LastValue,
    double Confidence,
    DateTime CreatedAt,
    DateTime? ConfirmedAt);