using System.Buffers.Text;
using System.Security.Authentication;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MeterVision.Api.Database;
using MeterVision.Api.Database.Models;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Services;

/// <summary>
/// Сервис аутентификации: проверяет пользователя по данным в базе.
/// </summary>
public class AuthService : IAuthService
{
    private record UserSession(Guid UserId, string Token);

    // Сессии пока хранятся в памяти, пользователи берутся из БД.
    private static readonly List<UserSession> ActiveSessions = new();
    private static readonly object SessionsLock = new();

    private readonly AppDatabase _db;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(AppDatabase db)
    {
        _db = db;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new AuthenticationException("Invalid login or password");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            throw new AuthenticationException("Invalid login or password");

        var check = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (check == PasswordVerificationResult.Failed)
            throw new AuthenticationException("Invalid login or password");

        var token = Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(32));
        lock (SessionsLock)
        {
            ActiveSessions.Add(new UserSession(user.Id, token));
        }
        return token;
    }

    public Task LogoutAsync(string token)
    {
        lock (SessionsLock)
        {
            var session = ActiveSessions.FirstOrDefault(s => s.Token == token);
            if (session == null)
                throw new InvalidOperationException("Invalid token or the session is already closed.");

            ActiveSessions.Remove(session);
        }
        return Task.CompletedTask;
    }

    public Task LogoutAllAsync(string token)
    {
        lock (SessionsLock)
        {
            var current = ActiveSessions.FirstOrDefault(s => s.Token == token);
            if (current == null)
                throw new InvalidOperationException("Invalid token");

            ActiveSessions.RemoveAll(s => s.UserId == current.UserId);
        }
        return Task.CompletedTask;
    }
}