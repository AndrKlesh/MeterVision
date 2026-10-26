using MeterVision.Api.Models.Domain;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Контракт службы для работы со счетчиками и агрегации показаний
/// </summary>
public interface IMeterService
{
    /// <summary>
    /// Получить счетчик со свежими показаниями по его идентификатору
    /// </summary>
    Task<MeterDetail?> GetMeterDetailsAsync(Guid meterId);

    /// <summary>
    /// Получить список всех счетчиков пользователя со свежими показаниями
    /// </summary>
    Task<IReadOnlyList<MeterDetail>> GetUserMetersAsync(Guid userId);
}
