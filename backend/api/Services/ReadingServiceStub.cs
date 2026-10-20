using MeterVision.Api.Models;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Заглушка сервиса показаний: всегда возвращает одни и те же фейковые данные.
/// </summary>
public class ReadingServiceStub : IReadingService
{
    public Task<RecognizeResponseDto> RecognizeAsync(Stream photo, string fileName)
    {
        var result = new RecognizeResponseDto(Guid.NewGuid(), 12345.6m, "SN-000123", 0.92, 0.88);
        return Task.FromResult(result);
    }

    public Task<ReadingDto> ConfirmAsync(Guid readingId, ConfirmReadingDto dto)
    {
        var result = new ReadingDto(readingId, dto.Value, dto.SerialNumber, "Confirmed", DateTime.UtcNow, DateTime.UtcNow);
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<ReadingDto>> GetListAsync(int page, int pageSize)
    {
        var list = new List<ReadingDto>
        {
            new(Guid.NewGuid(), 100.5m, "SN-000123", "Confirmed", DateTime.UtcNow, DateTime.UtcNow),
            new(Guid.NewGuid(), 120.0m, "SN-000123", "Pending", DateTime.UtcNow, null)
        };
        return Task.FromResult<IReadOnlyList<ReadingDto>>(list);
    }

    public Task<ReadingDto?> GetByIdAsync(Guid id)
    {
        var result = new ReadingDto(id, 12345.6m, "SN-000123", "Pending", DateTime.UtcNow, null);
        return Task.FromResult<ReadingDto?>(result);
    }
}