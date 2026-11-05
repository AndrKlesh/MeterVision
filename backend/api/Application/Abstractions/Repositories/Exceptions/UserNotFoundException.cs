namespace MeterVision.Api.Application.Abstractions.Repositories.Exceptions;

/// <summary>
/// Исключение, сигнализирующее о том, что запрошенный пользователь не найден.
/// </summary>
/// <remarks>
/// Бросается реализацией <see cref="IUserRepository"/> при попытке получить
/// пользователя по идентификатору или имени, если такого пользователя нет.
/// </remarks>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserNotFoundException"/>
    /// с сообщением по умолчанию.
    /// </summary>
    public UserNotFoundException()
        : base("Пользователь не найден.")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserNotFoundException"/>
    /// с пользовательским сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public UserNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserNotFoundException"/>
    /// с пользовательским сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение, ставшее причиной.</param>
    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
