namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services.Abstractions;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Username, dto.Password);

        if (!result.Success)
        {
            return Unauthorized(new { message = result.ErrorMessage });
        }

        return Ok(new { accessToken = result.token, tokenType = "Bearer" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
    {
        var success = await _authService.LogoutAsync(dto.token);

        if (!success)
        {
            return BadRequest(new { message = "Invalid token or the session is already closed." });
        }

        return Ok(new { message = "Successful logout" });
    }

    [HttpPost("logoutall")]
    public async Task<IActionResult> LogoutAll([FromBody] LogoutDto dto)
    {
        var success = await _authService.LogoutAllAsync(dto.token);

        if (!success)
        {
            return BadRequest(new { message = "Invalid token" });
        }

        return Ok(new { message = "All active sessions have been completed successfully" });
    }
}