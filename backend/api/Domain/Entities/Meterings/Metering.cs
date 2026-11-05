namespace MeterVision.Api.Domain.Entities.Meterings;

/// <summary>
/// Доменная сущность распознанного показания счётчика.
/// </summary>
/// <remarks>
/// Представляет результат автоматического распознавания показания с фотографии,
/// а также (опционально) подтверждённое оператором значение.
/// <para>
/// Сущность неизменяема (record): после создания экземпляра значения свойств
/// изменить нельзя. Для подтверждения показания создаётся новый экземпляр
/// с заполненными <see cref="ConfirmedValue"/> и <see cref="ConfirmedTimestamp"/>.
/// </para>
/// <para>
/// TODO: вынести <see cref="MeterSerialNumber"/> в value object
/// (<c>SerialNumber</c>) с валидацией формата; сейчас проверка отсутствует.
/// </para>
/// </remarks>
/// <param name="Id">Уникальный идентификатор записи о показании.</param>
/// <param name="UserId">Уникальный идентификатор пользователя отправившего фото показаний.</param>
/// <param name="MeterSerialNumber">Серийный номер счётчика, с которого снято показание.</param>
/// <param name="RecognizedValue">Значение, распознанное автоматически (может быть неточным).</param>
/// <param name="RecognizedTimestamp">Дата и время выполнения распознавания (UTC).</param>
/// <param name="Confidence">
/// Уверенность модели распознавания в результате, в диапазоне от 0 до 1.
/// Значение ниже порога (например, 0.7) обычно требует ручной проверки оператором.
/// </param>
/// <param name="ConfirmedValue">
/// Значение, подтверждённое оператором. <c>null</c>, если показание ещё не подтверждено.
/// </param>
/// <param name="ConfirmedTimestamp">
/// Дата и время подтверждения показания оператором (UTC).
/// Имеет смысл только при заполненном <see cref="ConfirmedValue"/>.
/// </param>
public record Metering(
    Guid Id,
    Guid UserId,
    string MeterSerialNumber,
    double RecognizedValue,
    DateTimeOffset RecognizedTimestamp,
    double Confidence,
    double? ConfirmedValue,
    DateTimeOffset ConfirmedTimestamp);
