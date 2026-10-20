namespace MeterVision.Api.Models;

/// <summary>
/// Данные, необходимые для выхода пользователя из системы.
/// </summary>
/// <param name="token">Токен аутентификации пользователя для выхода из всех сессий.</param>
public record LogoutDto(string token);