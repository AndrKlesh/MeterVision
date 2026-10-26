using MeterVision.Api.Database.Models;
using MeterVision.Api.Models.Domain;
using MeterVision.Api.Services.Abstractions;
using MeterVision.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Services;

/// <summary>
/// Служба для работы со счетчиками и агрегации показаний
/// </summary>
public class MeterService : IMeterService
{

    private readonly AppDatabase _dbContext;

    /// <summary>
    /// Конструктор службы работы со счетчиками
    /// </summary>
    /// <param name="dbContext">Контекст базы данных PostgreSQL</param>
    public MeterService(AppDatabase dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Получить счетчик с актуальными показаниями по идентификатору
    /// </summary>
    /// <param name="meterId">Уникальный идентификатор прибора учета</param>
    public async Task<MeterDetail?> GetMeterDetailsAsync(Guid meterId)
    {
        var meter = await _dbContext.Meters
        .Include(m => m.Records)
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.Id == meterId);

        if (meter == null)
        {
            return null;
        }

        var lastRecord = meter.Records
        .OrderByDescending(r => r.CreatedAt)
        .FirstOrDefault();

        var domainModel = MapToDomain(meter, lastRecord);

        return domainModel;
    }

    /// <summary>
    /// Получить список всех счетчиков пользователя с агрегированными данными
    /// </summary>
    /// <param name="userId">Идентификатор пользователя-владельца</param>
    public async Task<IReadOnlyList<MeterDetail>> GetUserMetersAsync(Guid userId)
    {
        var meters = await _dbContext.Meters
        .Include(m => m.Records)
        .AsNoTracking()
        .Where(m => m.UserId == userId)
        .ToListAsync();

        var result = new List<MeterDetail>();

        foreach (var meter in meters)
        {
            var lastRecord = meter.Records
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefault();

            result.Add(MapToDomain(meter, lastRecord));
        }

        return result;
    }

    /// <summary>
    /// Склейка двух сущностей СУБД (Meter и MeterRecord) в единую доменную модель
    /// </summary>
    /// <param name="meter">Сущность счетчика из базы данных</param>
    /// <param name="record">Сущность последнего замера (может быть null)</param>
    private static MeterDetail MapToDomain(Meter meter, MeterRecord? record)
    {
        string status = "Нет данных";
        if (record != null)
        {
            status = record.ConfirmedAt.HasValue ? "Подтверждено" : "Ожидает подтверждения";
        }

        return new MeterDetail
        {
            Id = meter.Id,
            SerialNumber = meter.SerialNumber,
            UserId = meter.UserId,
            LastValue = record?.ConfirmedValue ?? record?.RecognizedValue,
            Confidence = record?.ValueConfidence,
            Status = status,
            CreatedAt = record?.CreatedAt ?? meter.CreatedAt,
            ConfirmedAt = record?.ConfirmedAt
        };
    }
}
