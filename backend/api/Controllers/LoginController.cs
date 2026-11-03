
using Microsoft.AspNetCore.Mvc;

namespace MeterVision.Api.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей и управления сессиями.
/// </summary>
[ApiController]
[Route("api/v1/login")]
public class LoginController : ControllerBase
{
    /// <summary>
    /// Вход в систему по логину и паролю.
    /// </summary>
    /// <param name="dto">Учётные данные пользователя.</param>
    /// <returns>Токен доступа Bearer.</returns>
    /// <response code="200">Успешный вход в систему.</response>
    /// <response code="401">Неверный логин или пароль.</response>

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
    {
        // TODO: Реализовать позже
        return Ok();
    }

}
