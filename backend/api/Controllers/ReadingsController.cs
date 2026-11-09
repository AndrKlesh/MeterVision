using Mediator;
using MeterVision.Api.Application.Commands.Recognition;
using MeterVision.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MeterVision.Api.Controllers;

/// <summary>
/// Контроллер для работы с показаниями счётчиков: распознавание, подтверждение и чтение.
/// </summary>
/// <remarks>
/// Все методы требуют аутентификации через Basic Auth
/// </remarks>
[ApiController]
[Route("api/v1/readings")]
public class ReadingsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Ответ с результатами распознавания прибора учета
    /// </summary>
    /// <param name="Value">Распознанные показания счетчика</param>
    /// <param name="SerialNumber">Серийный номер счетчика</param>
    /// <param name="Confidence">Уровень уверенности распознавания от 0.0 до 1.0</param>
    public sealed record RecognitionResponseDto(string Value, string SerialNumber, double Confidence);

    /// <summary>
    /// DTO для подтверждения показания оператором.
    /// </summary>
    /// <param name="Id">Идентификатор записи.</param>
    /// <param name="SerialNumber">Серийный номер счётчика.</param>
    /// <param name="Value">Подтверждённое значение показания.</param>
    public sealed record ConfirmReadingDto(Guid Id, string SerialNumber, double Value);

    // <summary>
    /// DTO для неподтверждённой записи с фотографией в Base64.
    /// </summary>
    /// <param name="Id">Идентификатор записи.</param>
    /// <param name="SerialNumber">Серийный номер счётчика.</param>
    /// <param name="Value">Распознанное значение показания.</param>
    /// <param name="PhotoBase64">Фотография счётчика, закодированная в Base64.</param>    
    public sealed record UnconfirmedDataDto(Guid Id, string SerialNumber, double Value, string PhotoBase64);

    /// <summary>
    /// Отправить фотографию счётчика на распознавание.
    /// </summary>
    /// <param name="photo">Файл изображения (JPEG или PNG, не более 10 МБ).</param>
    /// <param name="cancellationToken">Токен отмены запроса.</param>
    /// <returns>Результат распознавания показаний.</returns>
    /// <response code="200">Распознавание выполнено успешно.</response>
    /// <response code="400">Файл пустой, слишком большой или неподдерживаемого формата.</response>
    [HttpPost("recognize")]
    public async Task<IActionResult> Recognize(IFormFile photo, CancellationToken cancellationToken)
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
        byte[] bytes = stream.ToArray();
        var result = await _mediator.Send(new RecognitionCommand(bytes), cancellationToken);
        var response = new RecognitionResponseDto(result.Value, result.SerialNumber, result.Confidence);
        return Ok(response);
    }

    /// <summary>
    /// Подтвердить показание оператором.
    /// </summary>
    /// <param name="dto">Данные для подтверждения.</param>
    /// <returns>Результат подтверждения.</returns>
    /// <response code="200">Показание подтверждено.</response>
    /// <response code="400">Некорректные данные показания.</response>
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm(ConfirmReadingDto dto, CancellationToken cancellationToken)
    {
        if (dto.Value < 0 || string.IsNullOrWhiteSpace(dto.SerialNumber))
        {
            return BadRequest("The reading data is invalid.");
        }
        var response = await _mediator.Send(new ConfirmCommand(dto.Id, dto.SerialNumber, dto.Value), cancellationToken);
        return Ok(response.Id);
    }

    /// <summary>
    /// Получить последнюю неподтверждённую запись показания.
    /// </summary>
    /// <returns>Последняя неподтверждённая запись.</returns>
    [HttpGet("unconfirmed")]
    public async Task<IActionResult> GetLastUnconfirmedValue()
    {
        throw new NotImplementedException();
    }

    // TODO: Сделать остальные методы. Пока реализованы заглушки.

    /// <summary>
    /// Получить список показаний с постраничной навигацией.
    /// </summary>
    /// <param name="page">Номер страницы (начиная с 1).</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <returns>Список показаний.</returns>
    [HttpGet]
    public async Task<IActionResult> GetList(int page = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получить одну запись показания по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <returns>Запись показания.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        throw new NotImplementedException();
    }

    private readonly IMediator _mediator = mediator;
}
