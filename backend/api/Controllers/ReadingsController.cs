using MeterVision.Api.Models;
using MeterVision.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MeterVision.Api.Controllers;

[ApiController]
[Route("api/v1/readings")]
public class ReadingsController : ControllerBase
{
    private readonly IReadingService _readingService;

    public ReadingsController(IReadingService readingService)
    {
        _readingService = readingService;
    }

    /// <summary>Отправить фото на распознавание.</summary>
    [HttpPost("recognize")]
    public async Task<IActionResult> Recognize(IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
        {
            return BadRequest("The file is empty.");
        }

        if (photo.Length > 10 * 1024 * 1024)
        {
            return BadRequest("The file is larger than 10 MB.");
        }

        if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
        {
            return BadRequest("The file must be a JPEG or PNG.");
        }

        using var stream = photo.OpenReadStream();
        var result = await _readingService.RecognizeAsync(stream, photo.FileName);
        return Ok(result);
    }

    /// <summary>Подтвердить показание.</summary>
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> Confirm(int id, ConfirmReadingDto dto)
    {   
        if (dto.Value < 0 || string.IsNullOrWhiteSpace(dto.SerialNumber))
        {
            return BadRequest("The reading data is invalid.");
        }

        var result = await _readingService.ConfirmAsync(id, dto);
        return Ok(result);
    }

    /// <summary>Получить список показаний.</summary>
    [HttpGet]
    public async Task<IActionResult> GetList(int page = 1, int pageSize = 10)
    {
        var result = await _readingService.GetListAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>Получить одну запись.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _readingService.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}