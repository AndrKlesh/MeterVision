namespace Api.Services;

using Api.Services.Abstractions;

public class AuthServiceStub : AuthService
{
    private record UserSession(string Username, string token);
    private static readonly List<UserSession> activeSessions = new();


    public Task<AuthResult> LoginAsync(string Username, string Password)
    {

        if (Username == "admin" && Password == "secret")
        {
            var fakeToken = "fake-" + Guid.NewGuid().ToString();
            activeSessions.Add(new UserSession(Username, fakeToken));
            return Task.FromResult(new AuthResult(true, fakeToken, null));
        }

        return Task.FromResult(new AuthResult(false, null, "Invalid login or password"));
    }

    public Task<bool> LogoutAsync(string token)
    {
        var session = activeSessions.FirstOrDefault(s => s.token == token);
        if (session != null)
        {
            activeSessions.Remove(session);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> LogoutAllAsync(string token)
    {
        var currentSession = activeSessions.FirstOrDefault(s => s.token == token);
        if (currentSession == null)
        {
            return Task.FromResult(false);
        }
        var targetUsername = currentSession.Username;
        activeSessions.RemoveAll(s => s.Username == targetUsername);
        return Task.FromResult(true);
    }
}