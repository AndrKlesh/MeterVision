using MeterVision.Api.DTO.Recognition;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Заглушка сервиса компьютерного зрения и файлового хранилища
/// </summary>
public class CvServiceStub : ICvService
{
    public async Task<RecognitionResponse> RecognizeAsync(IFormFile photo)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid():N}_{photo.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await photo.CopyToAsync(fileStream);
        }

        return new RecognitionResponse
        {
            Value = "50.99",
            SerialNumber = "51152054",
            Confidence = 0.98,
            PhotoUrl = $"/uploads/{uniqueFileName}"
        };
    }
}
