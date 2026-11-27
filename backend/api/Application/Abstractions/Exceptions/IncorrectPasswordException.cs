using MeterVision.Api.Application.Abstractions.Repositories;

namespace MeterVision.Api.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, сигнализирующее о том, что произошла ошибка аутентификации.
/// </summary>
/// <remarks>
/// Бросается реализацией <see cref="IUserRepository"/> при попытке получить
/// пароль по идентификатору или имени, если пароль неверный.
/// </remarks>
public class IncorrectPasswordException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="IncorrectPasswordException"/>
    /// с сообщением по умолчанию.
    /// </summary>
    public IncorrectPasswordException()
        : base("Неверный пароль.")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="IncorrectPasswordException"/>
    /// с пользовательским сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public IncorrectPasswordException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="IncorrectPasswordException"/>
    /// с пользовательским сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение, ставшее причиной.</param>
    public IncorrectPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
