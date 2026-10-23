using MeterVision.Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Database;

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
    /// Таблица счетчиков
    /// </summary>
    public DbSet<Meter> Meters => Set<Meter>();

    /// <summary>
    /// Таблица показаний счетчиков
    /// </summary>
    public DbSet<MeterRecord> MeterRecords => Set<MeterRecord>();

    /// <summary>
    /// Настройка связей между таблицами
    /// </summary>
    /// <param name="modelBuilder">Построитель модели базы данных</param>
    // этот метод Entity Framework вызывает сам, когда строит базу; override - значит мы дописываем свою настройку
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // вызываем настройку Entity Framework
        base.OnModelCreating(modelBuilder);

        // связь между пользователем и его счетчиками
        // настраиваем таблицу счетчиков
        modelBuilder.Entity<Meter>()
            // у счетчика есть один пользователь
            .HasOne(meter => meter.User)
            // а у пользователя может быть много счетчиков
            .WithMany()
            // связь хранится в поле UserId у счетчика
            .HasForeignKey(meter => meter.UserId);

        // связь между счетчиком и его записями показаний
        // настраиваем таблицу записей
        modelBuilder.Entity<MeterRecord>()
            // запись относится к одному счетчику
            .HasOne(record => record.Meter)
            // а со счетчика можно снять много показаний
            .WithMany(meter => meter.Records)
            // связь хранится в поле MeterId у записи
            .HasForeignKey(record => record.MeterId);
    }
}
