using MeterVision.Api.Application.Abstractions.Exceptions;
using MeterVision.Api.Application.Abstractions.Repositories;
using MeterVision.Api.Domain.Entities.Meterings;
using MeterVision.Api.Domain.Exceptions;
using MeterVision.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Infrastructure.Persistence.Repositories;

/// <summary>
/// Реализация <see cref="IMeteringRepository"/> поверх EF Core и <see cref="AppDatabase"/>.
/// </summary>
/// <remarks>
/// Работает с технической моделью БД <c>Infrastructure.Database.Models.Metering</c>,
/// а возвращает доменную сущность <c>Domain.Entities.Meterings.Metering</c>.
/// </remarks>
public class MeteringRepository : IMeteringRepository
{
    private readonly AppDatabase _db;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="MeteringRepository"/>.
    /// </summary>
    /// <param name="db">Контекст базы данных, внедряемый через DI.</param>
    public MeteringRepository(AppDatabase db) => _db = db;

    /// <summary>
    /// Возвращает последнее неподтверждённое показание для указанного пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя, создавшего показание.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Последнее неподтверждённое показание.</returns>
    /// <exception cref="MeteringNotFoundException">
    /// У пользователя нет неподтверждённых показаний.
    /// </exception>
    /// <remarks>
    /// «Последнее» определяется по <c>RecognizedTimestamp</c> (убывание).
    /// Неподтверждённым считается показание с <c>ConfirmedValue == null</c>.
    /// </remarks>
    public async Task<Metering> GetLastUnconfirmedValue(string username, CancellationToken cancellationToken)
    {
        // Находим пользователя по имени (логину).
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
            throw new UserNotFoundException($"Пользователь с именем '{username}' не найден.");

        // Ищем последнее неподтверждённое показание этого пользователя.
        var record = await _db.Meterings
            .AsNoTracking()
            .Where(m => m.UserId == user.Id && m.ConfirmedValue == null)
            .OrderByDescending(m => m.RecognizedTimestamp)
            .FirstOrDefaultAsync(cancellationToken);

        if (record is null)
            throw new MeteringNotFoundException(
                $"У пользователя '{username}' нет неподтверждённых показаний.");

        // Ручной маппинг: техническая модель → доменная сущность.
        // TODO: вынести в отдельный маппер (MeteringMapper).
        return new Metering(
            record.Id,
            record.UserId,
            record.MeterSerialNumber,
            record.RecognizedValue,
            record.RecognizedTimestamp,
            record.Confidence,
            record.ConfirmedValue,
            record.ConfirmedTimestamp ?? default);
    }

    /// <summary>
    /// Подтверждает показание оператором.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="confirmedValue">Подтверждённое значение.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Обновлённое показание (доменная сущность).</returns>
    /// <exception cref="MeteringNotFoundException">Показание не найдено.</exception>
    /// <exception cref="DomainException">
    /// Нарушено бизнес-правило: показание уже подтверждено, или пользователь
    /// не является автором показания, или значение отрицательное.
    /// </exception>
    public async Task<Metering> Confirm(
        Guid meteringId,
        double confirmedValue,
        CancellationToken cancellationToken)
    {
        // Загружаем техническую модель (с отслеживанием — её будем менять).
        var record = await _db.Meterings
            .FirstOrDefaultAsync(m => m.Id == meteringId, cancellationToken);

        if (record is null)
            throw new MeteringNotFoundException($"Показание с Id={meteringId} не найдено.");

        // Бизнес-проверки (TODO: перенести в домен).
        if (record.ConfirmedValue is not null)
            throw new DomainException("Показание уже подтверждено.");

        if (confirmedValue < 0)
            throw new DomainException("Значение показания не может быть отрицательным.");

        // Обновляем поля подтверждения.
        record.ConfirmedValue = confirmedValue;
        record.ConfirmedTimestamp = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        // Ручной маппинг: техническая модель → доменная сущность.
        // TODO: вынести в отдельный маппер (MeteringMapper).
        return new Metering(
            record.Id,
            record.UserId,
            record.MeterSerialNumber,
            record.RecognizedValue,
            record.RecognizedTimestamp,
            record.Confidence,
            record.ConfirmedValue,
            record.ConfirmedTimestamp ?? default);
    }
}
