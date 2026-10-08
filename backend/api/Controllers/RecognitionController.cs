using MeterVision.Api.DTO.Recognition;
using MeterVision.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MeterVision.Api.Controllers;

/// <summary>
/// Контроллер для распознавания показаний приборов учета
/// </summary>
[ApiController]
[Route("api/v1/recognition")]
public class RecognitionController : ControllerBase
{
    private readonly ICvService _cvService;

    public RecognitionController(ICvService cvService)
    {
        _cvService = cvService;
    }

    /// <summary>
    /// Загрузить фото счетчика и получить распознанные показания
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult<RecognitionResponse>> UploadPhoto(IFormFile photo)
    {
        var result = await _cvService.RecognizeAsync(photo);
        return Ok(result);
    }
}
