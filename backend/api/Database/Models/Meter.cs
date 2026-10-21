namespace MeterVision.Api.Database.Models;

/// <summary>
/// счетчик пользователя
/// </summary>
public class Meter
{
    /// <summary>
    /// Уникальный первичный ключ счетчика
    /// </summary>
    // генерируется сам
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// id пользователя, которому принадлежит счетчик
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь, которому принадлежит счетчик
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Серийный номер счетчика 
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время добавления счетчика
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Показания, снятые с этого счетчика
    /// </summary>
    public List<MeterReading> Readings { get; set; } = new();
}
