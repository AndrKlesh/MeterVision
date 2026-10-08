namespace MeterVision.Api.DTO.Auth;

/// <summary>
/// DTO для запроса аутентификации
/// </summary>
/// <remarks> DTO(Data Transport Object) - объект для передачи по протоколу связи, в нашем случае - HTTP</remarks>
public class LoginRequest
{
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; } = string.Empty;
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
