using MeterVision.Api.Database.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст базы данных приложения, который используется для взаимодействия с базой данных и управления сущностями
/// </summary>
namespace MeterVision.Api.Database;

/// <summary>
/// База данных приложения  и базовая структура 
/// </summary>
public class AppDatabase : DbContext
{
    /// <summary>
    /// Конструктор, в котором майкрософт сам пеоредаст настройки подключения к базе данных
    /// </summary>
    /// <param name="options">Опции подключения к базе данных</param>
    public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
    {
    }
    /// <summary>
    // Таблица пользователей в базе данных
    /// </summary>
    public DbSet<User> Users => Set<User>();

}
