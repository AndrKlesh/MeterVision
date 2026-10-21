namespace MeterVision.Api.Database.Models;

/// <summary>
/// Показание счетчика, переданное пользователем в систему
/// </summary>
public class MeterReading
{
    /// <summary>
    /// Уникальный первичный ключ показания
    /// </summary>
    // генерируется сам
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// id пользователя, который передал показание
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь, который передал показание
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// id счетчика. Может быть пустым, пока показание не привязаны к счетчику
    /// </summary>
    public Guid? MeterId { get; set; }

    /// <summary>
    /// Счетчик, к которому относится показание
    /// </summary>
    public Meter? Meter { get; set; }

    /// <summary>
    /// Путь к фотографии 
    /// </summary>
    public string PhotoPath { get; set; } = string.Empty;

    /// <summary>
    /// Значение показания, которое распознал CV-сервис. Пустое, если распознать не удалось
    /// </summary>
    public decimal? RecognizedValue { get; set; }

    /// <summary>
    /// Серийный номер, который распознал CV-сервис. тоже пустой, если распознать не удалось
    /// </summary>
    public string? RecognizedSerialNumber { get; set; }

    /// <summary>
    /// Уверенность распознавания значения 
    /// </summary>
    public double? ValueConfidence { get; set; }

    /// <summary>
    /// Уверенность распознавания серийного номера 
    /// </summary>
    public double? SerialNumberConfidence { get; set; }

    /// <summary>
    /// Значение показания
    /// </summary>
    // пока пользователь не подтвердил, тут null
    public decimal? ConfirmedValue { get; set; }

    /// <summary>
    /// Серийный номер
    /// </summary>
    // пока пользователь не подтвердил, тут null
    public string? ConfirmedSerialNumber { get; set; }

    /// <summary>
    /// Статус записи показания счетчика: ожидает подтверждения, подтверждено или отменено
    /// </summary>
    public ReadingStatus Status { get; set; } = ReadingStatus.PendingConfirmation;

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время подтверждения. Пустое если показание еще не подтверждено пользователем
    /// </summary>
    // пока пользователь не подтвердил, тут null
    public DateTime? ConfirmedAt { get; set; }
}
