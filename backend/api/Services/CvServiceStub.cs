using MeterVision.Api.Application.Commands.Recognition;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Заглушка сервиса компьютерного зрения и файлового хранилища
/// </summary>
public class CvServiceStub : ICvService
{
    public Task<RecognitionResponse> RecognizeAsync(RecognitionCommand recognitionCommand)
    {
        var response = new RecognitionResponse("50.99", "51152054", 0.98);

        return Task.FromResult(response);
    }
}
