namespace MeterVision.Api.Infrastructure.Database.Models;

/// <summary>
/// сущность пользователя системы для сервиса аутентификации
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный первичный ключ пользователя
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля,чтобы пароль в закрытом доступе был(зашифрованным)
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
}

