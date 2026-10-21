namespace MeterVision.Api.Database.Models;

/// <summary>
/// Статус записи показания счетчика
/// </summary>
public enum ReadingStatus
{
    /// <summary>
    /// Ожидает подтверждения
    /// </summary>
    PendingConfirmation = 0,

    /// <summary>
    /// Подтверждено
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Отменено
    /// </summary>
    Canceled = 2
}
