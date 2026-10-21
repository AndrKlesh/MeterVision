using MeterVision.Api.Models;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

public class ReadingServiceStub : IReadingService
{
    public Task<RecognizeResponseDto> RecognizeAsync(Stream photo, string fileName)
    {
        var result = new RecognizeResponseDto(1, 12345.6, "SN-000123", 0.92);
        return Task.FromResult(result);
    }

    public Task<MeterDetailDto> ConfirmAsync(int id, ConfirmReadingDto dto)
    {
        var result = new MeterDetailDto(id, dto.SerialNumber, "вода", "Подтверждено", dto.Value, 0.92, DateTime.UtcNow, DateTime.UtcNow);
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<MeterDetailDto>> GetListAsync(int page, int pageSize)
    {
        var list = new List<MeterDetailDto>
        {
            new(1, "SN-000123", "вода", "Подтверждено", 100.5, 0.95, DateTime.UtcNow, DateTime.UtcNow),
            new(2, "SN-000456", "электро", "Ожидает", 2300.0, 0.88, DateTime.UtcNow, null)
        };
        return Task.FromResult<IReadOnlyList<MeterDetailDto>>(list);
    }

    public Task<MeterDetailDto?> GetByIdAsync(int id)
    {
        var result = new MeterDetailDto(id, "SN-000123", "вода", "Ожидает", 12345.6, 0.92, DateTime.UtcNow, null);
        return Task.FromResult<MeterDetailDto?>(result);
    }
}