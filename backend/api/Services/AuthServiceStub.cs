using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

public class AuthServiceStub : IAuthService
{
    public string Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new UnauthorizedAccessException();
        }

        return Guid.NewGuid().ToString("N");
    }
}
