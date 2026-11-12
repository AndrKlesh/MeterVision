using MeterVision.Api.Application.Abstractions.Repositories;

namespace MeterVision.Api.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, сигнализирующее о том, что запрошенное показание не найдено.
/// </summary>
/// <remarks>
/// Бросается реализацией <see cref="IMeteringRepository"/> при попытке получить
/// показание (или последнее неподтверждённое показание), если такого нет.
/// На уровне API обычно преобразуется в HTTP 404 Not Found.
/// </remarks>
public class MeteringNotFoundException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="MeteringNotFoundException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public MeteringNotFoundException(string message) : base(message)
    {
    }
}
