namespace MeterVision.Api.Database.Models;

/// <summary>
/// Запись показания счетчика: фото, распознанные и подтвержденные значения
/// </summary>
public class MeterRecord
{
    /// <summary>
    /// Уникальный первичный ключ записи
    /// </summary>
    // генерируется сам
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// id счетчика, к которому относится запись
    /// </summary>
    // пользователя здесь не храним: он известен через счетчик
    public Guid MeterId { get; set; }

    /// <summary>
    /// Счетчик, к которому относится запись
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
    /// Серийный номер, который распознал CV-сервис. Тоже пустой, если распознать не удалось
    /// </summary>
    // это то, что прочитал CV-сервис; настоящий серийный номер хранится у счетчика
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
    /// Значение показания, подтвержденное пользователем
    /// </summary>
    // пока пользователь не подтвердил, тут null
    public decimal? ConfirmedValue { get; set; }

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// если ConfirmedAt пустое, показание ждет подтверждения, если заполнено - подтверждено. Если пользователь отменил показание, запись удаляется.
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }
}
