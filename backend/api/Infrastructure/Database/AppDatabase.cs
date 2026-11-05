using MeterVision.Api.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Infrastructure.Database;

/// <summary>
/// Контекст базы данных приложения: таблицы и связи между ними
/// </summary>
// наследуем DbContext, чтобы Entity Framework работал с нашей бд
public class AppDatabase : DbContext
{
    /// <summary>
    /// Конструктор, в который ASP.NET Core сам передаст настройки подключения к базе данных
    /// </summary>
    /// <param name="options">Опции подключения к базе данных</param>
    // настройки (например, что база PostgreSQL и где она лежит) задаются в Program.cs
    // через base передаем их в DbContext
    public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
    {
    }

    /// <summary>
    /// Таблица пользователей
    /// </summary>
    // через  DbSet  добавляем и ищем наши классы, 
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Настройка связей между таблицами
    /// </summary>
    /// <param name="modelBuilder">Построитель модели базы данных</param>
    // этот метод Entity Framework вызывает сам, когда строит базу; override - значит мы дописываем свою настройку
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // вызываем настройку Entity Framework
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            // Уникальность Username — ключевое требование для аутентификации
            entity.HasIndex(u => u.Username)
                  .IsUnique();
        });
    }
}
