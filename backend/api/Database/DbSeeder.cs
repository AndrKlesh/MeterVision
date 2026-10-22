
using Microsoft.AspNetCore.Identity;

using MeterVision.Api.Database.Models;

namespace MeterVision.Api.Database;

/// <summary>
/// Добавляет тестового пользователя.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDatabase db)
    {
        var admin = new User
        {
            Username = "admin",
            Email = "admin@mail.ru"
        };

        admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, "admin");

        db.Users.Add(admin);
        db.SaveChanges();
    }
}