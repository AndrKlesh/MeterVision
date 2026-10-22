namespace MeterVision.Api.Models;

/// <summary>
/// DTO для выхода пользователя из системы.
/// </summary>
/// <param name="token">Токен доступа для завершения сессии.</param>
public record LogoutDto(string token);
