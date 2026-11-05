using MeterVision.Api.Application.Abstractions.Repositories;
using MeterVision.Api.Application.Abstractions.Services;

namespace MeterVision.Api.Infrastructure.Services;

public class AuthService(IUserRepository _userRepository, ILogger<AuthService> _logger) : IAuthService
{
    public async Task<string> LoginAsync(string Username, string Password)
    {
        try
        {
            var user = await _userRepository.Get(Username, default);
            if (Username == user.Username && Password == user.PasswordHash)
            {
                var fakeToken = "fake-" + Guid.NewGuid().ToString();
                activeSessions.Add(new UserSession(Username, fakeToken));
                return fakeToken;
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Сбой аутентификации пользователя User=[{username}:{password}]", Username, Password);
        }


        return "Invalid login or password";
    }

    public Task LogoutAsync(string token)
    {
        var session = activeSessions.FirstOrDefault(s => s.token == token);
        if (session != null)
        {
            activeSessions.Remove(session);
            return Task.CompletedTask;
        }
        throw new InvalidOperationException("Invalid token or the session is already closed.");
    }

    public Task LogoutAllAsync(string token)
    {
        var currentSession = activeSessions.FirstOrDefault(s => s.token == token);
        if (currentSession == null)
        {
            throw new InvalidOperationException("Invalid token");
        }
        var targetUsername = currentSession.Username;
        activeSessions.RemoveAll(s => s.Username == targetUsername);
        return Task.CompletedTask;
    }

    private static readonly List<UserSession> activeSessions = [];
    private record UserSession(string Username, string token);
}
