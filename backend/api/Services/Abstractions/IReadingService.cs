using MeterVision.Api.Models;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Контракт сервиса работы с показаниями приборов учёта.
/// </summary>
public interface IReadingService
{
    /// <summary>Распознаёт показания по фото и создаёт черновик записи.</summary>
    Task<RecognizeResponseDto> RecognizeAsync(Stream photo, string fileName);

    /// <summary>Сохраняет данные, подтверждённые пользователем.</summary>
    Task<ReadingDto> ConfirmAsync(Guid readingId, ConfirmReadingDto dto);

    /// <summary>Возвращает страницу списка показаний.</summary>
    Task<IReadOnlyList<ReadingDto>> GetListAsync(int page, int pageSize);

    /// <summary>Возвращает одну запись или null, если её нет.</summary>
    Task<ReadingDto?> GetByIdAsync(Guid id);
}