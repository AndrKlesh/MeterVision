namespace MeterVision.Api.Models;

/// <summary>
/// DTO для входа пользователя в систему.
/// </summary>
/// <param name="Username">Имя пользователя (логин).</param>
/// <param name="Password">Пароль пользователя.</param>
public record LoginDto(string Username, string Password);
