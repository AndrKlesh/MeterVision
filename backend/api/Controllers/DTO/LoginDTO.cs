namespace MeterVision.Api.Controllers.DTO;

/// <summary>
/// DTO для аутентификации пользователя.
/// </summary>
/// <param name="Username">Имя пользователя</param>
/// <param name="Password">Пароль</param>
public record LoginDto(string Username, string Password);
