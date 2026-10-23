using MeterVision.Api.Models.Domain;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Служба для работы со счетчиками и агрегации показаний
/// </summary>
public class MeterService : IMeterService
{
    /// <summary>
    /// Сущность прибора учета уровня данных для маппинга
    /// </summary>
    public record MeterDbEntity(Guid Id, Guid UserId, string SerialNumber);

    /// <summary>
    /// Сущность показания прибора учета уровня данных для маппинга
    /// </summary>
    public record ReadingDbEntity(Guid Id, Guid MeterId, decimal? Value, double? Confidence, string Status, DateTime CreatedAt, DateTime? ConfirmedAt);

    /// <summary>
    /// Временное хранилище приборов учета до интеграции контекста СУБД
    /// </summary>
    private static readonly List<MeterDbEntity> DbMeters = new()
    {
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "51152054"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "09604312")
    };

    /// <summary>
    /// Временное хранилище показаний до интеграции контекста СУБД
    /// </summary>
    private static readonly List<ReadingDbEntity> DbReadings = new()
    {
        new(Guid.NewGuid(), Guid.Parse("11111111-1111-1111-1111-111111111111"), 50.99m, 0.98, "Подтверждено", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow),
        new(Guid.NewGuid(), Guid.Parse("22222222-2222-2222-2222-222222222222"), 124.50m, 0.95, "Ожидает", DateTime.UtcNow, null)
    };

    /// <summary>
    /// Получить счетчик с актуальными показаниями по идентификатору
    /// </summary>
    public Task<MeterDetail?> GetMeterDetailsAsync(Guid meterId)
    {
        var meter = DbMeters.FirstOrDefault(m => m.Id == meterId);
        if (meter == null)
        {
            return Task.FromResult<MeterDetail?>(null);
        }

        var lastReading = DbReadings
            .Where(r => r.MeterId == meterId)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();

        var domainModel = MapToDomain(meter, lastReading);

        return Task.FromResult<MeterDetail?>(domainModel);
    }

    /// <summary>
    /// Получить список всех счетчиков пользователя с агрегированными данными
    /// </summary>
    public Task<IReadOnlyList<MeterDetail>> GetUserMetersAsync(Guid userId)
    {
        var userMeters = DbMeters.Where(m => m.UserId == userId).ToList();
        var result = new List<MeterDetail>();

        foreach (var meter in userMeters)
        {
            var lastReading = DbReadings
                .Where(r => r.MeterId == meter.Id)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefault();

            result.Add(MapToDomain(meter, lastReading));
        }

        return Task.FromResult<IReadOnlyList<MeterDetail>>(result);
    }

    /// <summary>
    /// Склейка сущностей уровня данных в единую доменную модель
    /// </summary>
    private static MeterDetail MapToDomain(MeterDbEntity meter, ReadingDbEntity? reading)
    {
        return new MeterDetail
        {
            Id = meter.Id,
            SerialNumber = meter.SerialNumber,
            UserId = meter.UserId,
            LastValue = reading?.Value,
            Confidence = reading?.Confidence,
            Status = reading?.Status ?? "Нет данных",
            CreatedAt = reading?.CreatedAt ?? DateTime.UtcNow,
            ConfirmedAt = reading?.ConfirmedAt
        };
    }
}
