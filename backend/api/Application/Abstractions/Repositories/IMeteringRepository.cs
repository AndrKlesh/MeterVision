using MeterVision.Api.Application.Abstractions.Exceptions;
using MeterVision.Api.Domain.Entities.Meterings;

namespace MeterVision.Api.Application.Abstractions.Repositories;

/// <summary>
/// Контракт репозитория для работы с показаниями счётчиков.
/// </summary>
public interface IMeteringRepository
{
    /// <summary>
    /// Возвращает последнее неподтверждённое показание для указанного пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя, создавшего показание.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Последнее неподтверждённое показание.</returns>
    /// <exception cref="MeteringNotFoundException">
    /// У пользователя нет неподтверждённых показаний.
    /// </exception>
    public Task<Metering> GetLastUnconfirmedValue(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Подтверждает показание оператором.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="confirmedValue">Подтверждённое значение.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Обновлённое показание.</returns>
    /// <exception cref="MeteringNotFoundException">Показание не найдено.</exception>
    /// Нарушено бизнес-правило (например, показание уже подтверждено
    /// или пользователь не является автором).
    /// </exception>
    public Task<Metering> Confirm(
        Guid meteringId,
        double confirmedValue,
        CancellationToken cancellationToken);
}
