using System.Security.Authentication;

namespace MeterVision.Api.Services.Abstractions;

/// <summary>
/// Контракт сервиса аутентификации.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Выполняет вход пользователя в систему.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <param name="password">Пароль.</param>
    /// <returns>Строка с токеном доступа.</returns>
    /// <exception cref="AuthenticationException">Выбрасывается при неверном логине или пароле.</exception>
    Task<string> LoginAsync(string username, string password);

    /// <summary>
    /// Завершает текущую сессию.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если сессия не найдена.</exception>
    Task LogoutAsync(string token);

    /// <summary>
    /// Завершает все активные сессии пользователя.
    /// </summary>
    /// <param name="token">Токен текущей сессии.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если сессия не найдена.</exception>
    Task LogoutAllAsync(string token);
}