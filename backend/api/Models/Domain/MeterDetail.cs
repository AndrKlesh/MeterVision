namespace MeterVision.Api.Models.Domain;

/// <summary>
/// Доменная модель прибора учета, объединяющая данные счетчика и его показаний
/// </summary>
public class MeterDetail
{
    /// <summary>
    /// Идентификатор счетчика
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Серийный номер счетчика
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор пользователя-владельца
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Статус последнего показания (Ожидает / Подтверждено / Отменено)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Числовое значение последнего показания
    /// </summary>
    public decimal? LastValue { get; set; }

    /// <summary>
    /// Оценка уверенности распознавания (0.0 - 1.0)
    /// </summary>
    public double? Confidence { get; set; }

    /// <summary>
    /// Дата фиксации показания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата подтверждения пользователем
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }
}
