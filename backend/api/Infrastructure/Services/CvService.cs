using MeterVision.Api.Application.Abstractions.Services;
using MeterVision.Api.Application.Commands.Recognition;

namespace MeterVision.Api.Infrastructure.Services;

/// <summary>
/// Заглушка сервиса компьютерного зрения и файлового хранилища
/// </summary>
public class CvService(
    IMeteringService _meteringService,
    IImageService _imageService) : ICvService
{
    public Task<RecognitionResponse> RecognizeAsync(RecognitionCommand recognitionCommand)
    {
        // TODO: Реализовать сервис
        var response = new RecognitionResponse("50.99", "51152054", 0.98);

        return Task.FromResult(response);
    }
}
