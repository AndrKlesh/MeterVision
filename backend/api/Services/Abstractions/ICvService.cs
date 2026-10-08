using MeterVision.Api.DTO.Recognition;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Контракт сервиса для обработки и распознавания показаний счетчиков
/// </summary>
public interface ICvService
{
    /// <summary>
    /// Принимает фотографию счетчика, сохраняет ее и возвращает результат распознавания
    /// </summary>
    /// <param name="photo">Файл изображения от пользователя</param>
    Task<RecognitionResponse> RecognizeAsync(IFormFile photo);
}