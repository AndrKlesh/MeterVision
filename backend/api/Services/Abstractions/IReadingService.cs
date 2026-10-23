
using MeterVision.Api.Models;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Определяет операции работы с показаниями приборов учёта.
/// </summary>
public interface IReadingService
{
    /// <summary>Распознаёт показания по фотографии.</summary>
    /// <param name="photo">Фотография прибора.</param>
    /// <param name="fileName">Имя файла.</param>
    Task<RecognizeResponseDto> RecognizeAsync(Stream photo, string fileName);

    /// <summary>Подтверждает показания.</summary>
    /// <param name="id">Идентификатор показания.</param>
    /// <param name="dto">Данные для подтверждения.</param>
    Task<MeterDetailDto> ConfirmAsync(int id, ConfirmReadingDto dto);

    /// <summary>Возвращает список показаний.</summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    Task<IReadOnlyList<MeterDetailDto>> GetListAsync(int page, int pageSize);

    /// <summary>Возвращает показание по идентификатору.</summary>
    /// <param name="id">Идентификатор показания.</param>
    Task<MeterDetailDto?> GetByIdAsync(int id);
}
