
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using MeterVision.Api.Models;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей и управления сессиями.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Вход в систему по логину и паролю.
    /// </summary>
    /// <param name="dto">Учётные данные пользователя.</param>
    /// <returns>Токен доступа Bearer.</returns>
    /// <response code="200">Успешный вход в систему.</response>
    /// <response code="401">Неверный логин или пароль.</response>
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto.Username, dto.Password);
            return Ok(new { accessToken = token, tokenType = "Bearer" });
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Завершение текущей сессии (выход с текущего устройства).
    /// </summary>
    /// <param name="dto">Токен активной сессии.</param>
    /// <returns>Результат завершения сессии.</returns>
    /// <response code="200">Сессия успешно завершена.</response>
    /// <response code="400">Недействительный токен или сессия уже завершена.</response>
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
    {
        try
        {
            await _authService.LogoutAsync(dto.token);
            return Ok(new { message = "Successful logout" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Завершение всех активных сессий пользователя.
    /// </summary>
    /// <param name="dto">Токен текущей сессии.</param>
    /// <returns>Результат завершения всех сессий.</returns>
    /// <response code="200">Все сессии успешно завершены.</response>
    /// <response code="400">Недействительный токен.</response>
    
    [HttpPost("logoutall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogoutAll([FromBody] LogoutDto dto)
    {
        try
        {
            await _authService.LogoutAllAsync(dto.token);
            return Ok(new { message = "All active sessions have been completed successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}