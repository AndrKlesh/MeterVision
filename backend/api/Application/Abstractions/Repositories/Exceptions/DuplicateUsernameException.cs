namespace MeterVision.Api.Application.Abstractions.Repositories.Exceptions;

/// <summary>
/// Исключение, сигнализирующее о нарушении уникальности имени пользователя.
/// </summary>
/// <remarks>
/// Бросается реализацией <see cref="IUserRepository"/> при попытке сохранить
/// пользователя, если имя (логин) уже занято другим пользователем.
/// </remarks>
public class DuplicateUsernameException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DuplicateUsernameException"/>
    /// с сообщением по умолчанию.
    /// </summary>
    public DuplicateUsernameException()
        : base("Пользователь с таким именем уже существует.")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DuplicateUsernameException"/>
    /// с пользовательским сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public DuplicateUsernameException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DuplicateUsernameException"/>
    /// с пользовательским сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение, ставшее причиной.</param>
    public DuplicateUsernameException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
