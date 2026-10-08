namespace MeterVision.Api.Database.Models;

/// <summary>
/// Запись с показаниями
/// </summary>
public class MeterReading
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Ссылки на пользователя 
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid MeterId { get; set; }
    public Meter? Meter { get; set; }

    // Ссылка на фото на сервере (хранится вне БД)
    public string PhotoUrl { get; set; } = string.Empty;

    // Данные от нейросети
    public string RecognizedValue { get; set; } = string.Empty;
    public string RecognizedSerialNumber { get; set; } = string.Empty;
    public double Confidence { get; set; }

    // Данные, подтвержденные пользователем
    public string? ConfirmedValue { get; set; }
    public string? ConfirmedSerialNumber { get; set; }

    // Статус записи (черновик / подтверждено / отменено)
    public ReadingStatus Status { get; set; } = ReadingStatus.WaitingForConfirmation;

    // Временные метки создания и подтверждения
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
}
