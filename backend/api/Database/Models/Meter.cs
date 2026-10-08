namespace MeterVision.Api.Database.Models;

/// <summary>
/// Прибор учета (счетчик) пользователя
/// </summary>
public class Meter
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Ссылка на владельца(храним айдишник из user)
    public Guid UserId { get; set; }

    public User? User { get; set; }

    // Серийный номер счетчика
    public string SerialNumber { get; set; } = string.Empty;

    // Тип: "Холодная вода", "Горячая вода", "Электричество"
    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Связь: история показаний этого счетчика
    public List<MeterReading> Readings { get; set; } = new();
}
