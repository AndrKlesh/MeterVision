namespace MeterVision.Api.Services;

using MeterVision.Api.Services.Abstractions;

public class AuthServiceStub : IAuthService
{
    private record UserSession(string Username, string token);
    private static readonly List<UserSession> activeSessions = new();


    public Task<string> LoginAsync(string Username, string Password)
    {

        if (Username == "admin" && Password == "secret")
        {
            var fakeToken = "fake-" + Guid.NewGuid().ToString();
            activeSessions.Add(new UserSession(Username, fakeToken));
            return Task.FromResult(fakeToken);
        }

        return Task.FromResult("Invalid login or password");
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
}