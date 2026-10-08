using MeterVision.Api.DTO.Recognition;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Заглушка сервиса компьютерного зрения и файлового хранилища
/// </summary>
public class CvServiceStub : ICvService
{
    public Task<RecognitionResponse> RecognizeAsync(IFormFile photo)
    {
        var response = new RecognitionResponse
        {
            Value = "50.99",
            SerialNumber = "51152054",
            Confidence = 0.98
        };

        return Task.FromResult(response);
    }
}
