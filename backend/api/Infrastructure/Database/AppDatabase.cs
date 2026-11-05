using MeterVision.Api.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Infrastructure.Database;

/// <summary>
/// Контекст базы данных приложения: таблицы и связи между ними.
/// </summary>
public class AppDatabase : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AppDatabase"/>.
    /// </summary>
    /// <param name="options">Опции подключения к базе данных.</param>
    public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
    {
    }

    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Таблица распознанных показаний счётчиков.
    /// </summary>
    public DbSet<Metering> Meterings => Set<Metering>();

    /// <summary>
    /// Настройка модели базы данных: связи, ограничения, индексы.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели базы данных.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            // Уникальность Username — ключевое требование для аутентификации
            entity.HasIndex(u => u.Username)
                  .IsUnique();
        });

        modelBuilder.Entity<Metering>(entity =>
        {
            // TODO: добавить ограничение длины для MeterSerialNumber
            // и индекс по MeterSerialNumber для быстрого поиска показаний по счётчику.
        });
    }
}
