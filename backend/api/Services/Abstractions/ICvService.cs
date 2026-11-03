using MeterVision.Api.Application.Commands.Recognition;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Контракт сервиса для обработки и распознавания показаний счетчиков
/// </summary>
public interface ICvService
{
    /// <summary>
    /// Принимает фотографию счетчика, сохраняет ее и возвращает результат распознавания
    /// </summary>
    /// <param name="recognitionCommand">Файл изображения от пользователя</param>
    Task<RecognitionResponse> RecognizeAsync(RecognitionCommand recognitionCommand);
}
