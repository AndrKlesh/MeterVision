namespace MeterVision.Api.DTO.Auth;

/// <summary>
/// DTO для ответа на запрос аутентификации
/// </summary>
/// <remarks> DTO(Data Transport Object) - объект для передачи по протоколу связи, в нашем случае - HTTP</remarks>
public class LoginResponse
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
