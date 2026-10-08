namespace MeterVision.Api.Services.Abstractions;

public record AuthResult(
    bool Success, 
    string? token, 
    string? ErrorMessage
);

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string Username, string Password);
    Task<bool> LogoutAsync(string token);
    Task<bool> LogoutAllAsync(string token);
}