namespace MeterVision.Api.Domain.Entities.Auth;

/// <summary>
/// Доменная сущность пользователя системы.
/// </summary>
/// </remarks>
/// <param name="Id">Уникальный идентификатор пользователя.</param>
/// <param name="Username">Имя пользователя (логин), используемое для входа.</param>
/// <param name="PasswordHash">
/// Хэш пароля. Пароль в открытом виде не хранится и не передаётся в сущности.
/// </param>
public record User(int Id, string Username, string PasswordHash);
