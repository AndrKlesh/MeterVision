namespace MeterVision.Api.Controllers.DTO;

/// <summary>
/// DTO для функции выхода пользователя.
/// </summary>
/// <param name="Token">Аутентификационный токен пользователя.</param>
public record LogoutDto(string Token);
