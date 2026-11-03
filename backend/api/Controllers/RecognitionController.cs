using Mediator;
using MeterVision.Api.Application.Commands.Recognition;
using MeterVision.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MeterVision.Api.Controllers;

/// <summary>
/// Контроллер для распознавания показаний приборов учета
/// </summary>
[ApiController]
[Route("api/v1/recognition")]
public class RecognitionController : ControllerBase
{
    public RecognitionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Загрузить фото счетчика и получить распознанные показания
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult<RecognitionResponse>> UploadPhoto(IFormFile photo, CancellationToken cancellationToken)
    {
        using var stream = photo.OpenReadStream();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        byte[] bytes = ms.ToArray();
        var result = await _mediator.Send(new RecognitionCommand(bytes), cancellationToken);
        return Ok(result);
    }

    private readonly IMediator _mediator;
}
