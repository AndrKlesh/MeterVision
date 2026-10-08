using MeterVision.Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterVision.Api.Database;


// База данных приложения  и базовая структура 

public class AppDatabase : DbContext
{
    //конструктор, в котором майкрософт сам пеоредаст настройки подключения к базе данных
    public AppDatabase(DbContextOptions<AppDatabase> options) : base(options)
    {
    }

    // Таблицы базы данных
    public DbSet<User> Users => Set<User>();
    public DbSet<Meter> Meters => Set<Meter>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();
}
