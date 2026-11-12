namespace MeterVision.Api.Infrastructure.Database.Models;

/// <summary>
/// Техническая модель БД для распознанного показания счётчика.
/// </summary>
/// <remarks>
/// Хранит результат распознавания и (опционально) подтверждённое оператором значение.
/// Не является доменной сущностью: если появится доменная логика (например, правила
/// подтверждения), её следует вынести в Domain, а эту модель оставить только для хранения.
/// </remarks>
public class Metering
{
    /// <summary>
    /// Уникальный первичный ключ записи о показании.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор пользователя, отправившего фото для распознавания.
    /// </summary>
    /// <remarks>
    /// TODO: добавить навигационное свойство <c>User</c> и внешний ключ
    /// в конфигурации EF Core, чтобы можно было загружать пользователя вместе с показанием.
    /// </remarks>
    public Guid UserId { get; set; }

    /// <summary>
    /// Серийный номер счётчика, с которого снято показание.
    /// </summary>
    /// <remarks>
    /// TODO: добавить ограничение длины и, возможно, внешний ключ на таблицу счётчиков.
    /// </remarks>
    public string MeterSerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Значение, распознанное автоматически (может быть неточным).
    /// </summary>
    public double RecognizedValue { get; set; }

    /// <summary>
    /// Дата и время выполнения распознавания (UTC).
    /// </summary>
    public DateTimeOffset RecognizedTimestamp { get; set; }

    /// <summary>
    /// Уверенность модели распознавания в результате, в диапазоне от 0 до 1.
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Значение, подтверждённое оператором. <c>null</c>, если показание ещё не подтверждено.
    /// </summary>
    public double? ConfirmedValue { get; set; }

    /// <summary>
    /// Дата и время подтверждения показания оператором (UTC).
    /// </summary>
    public DateTimeOffset? ConfirmedTimestamp { get; set; }
}
