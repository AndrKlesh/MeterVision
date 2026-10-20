
namespace MeterVision.Api.Models;

/// <summary>
/// Данные, необходимые для аутентификации пользователя.
/// </summary>
/// <param name="Username">Имя пользователя.</param>
/// <param name="Password">Пароль пользователя.</param>
public record LoginDto(string Username, string Password);
