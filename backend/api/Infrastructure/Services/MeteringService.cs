using MeterVision.Api.Application.Abstractions.Exceptions;
using MeterVision.Api.Application.Abstractions.Repositories;
using MeterVision.Api.Application.Abstractions.Services;
using MeterVision.Api.Domain.Entities.Meterings;
using MeterVision.Api.Domain.Exceptions;

namespace MeterVision.Api.Infrastructure.Services;

/// <summary>
/// Реализация <see cref="IMeteringService"/>.
/// </summary>
/// <remarks>
/// Координирует работу с <see cref="IMeteringRepository"/> и применяет
/// бизнес-правила, которые не сводятся к простому чтению.
///
/// Инициализирует новый экземпляр <see cref="MeteringService"/>.
/// </remarks>
/// <param name="_repository">Репозиторий показаний.</param>
public class MeteringService(IMeteringRepository _repository) : IMeteringService
{
    /// <summary>
    /// Возвращает последнее неподтверждённое показание для указанного пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Последнее неподтверждённое показание.</returns>
    /// <exception cref="MeteringNotFoundException">
    /// У пользователя нет неподтверждённых показаний.
    /// </exception>
    public Task<Metering> GetLastUnconfirmed(string username, CancellationToken cancellationToken)
        => _repository.GetLastUnconfirmedValue(username, cancellationToken);

    /// <summary>
    /// Подтверждает показание оператором.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="confirmedValue">Подтверждённое значение.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Обновлённое показание.</returns>
    /// <exception cref="MeteringNotFoundException">Показание не найдено.</exception>
    /// <exception cref="DomainException">
    /// Показание уже подтверждено, или пользователь не является автором.
    /// </exception>
    public async Task<Metering> Confirm(
          Guid meteringId,
          double confirmedValue,
          CancellationToken cancellationToken)
    {

        return await _repository.Confirm(meteringId, confirmedValue, cancellationToken);
    }
}
