namespace MeterVision.Api.Database.Models;



public class User
{
    // Уникальный первичный ключ пользователя
    public Guid Id { get; set; } = Guid.NewGuid();
    // Имя пользователя
    public string Username { get; set; } = string.Empty;
    // почта
    public string Email { get; set; } = string.Empty;
    // Хэш пароля,чтобы пароль в закрытом доступе был(зашифрованным)
    public string PasswordHash { get; set; } = string.Empty;
    //Дата и время создания аккаунта
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

