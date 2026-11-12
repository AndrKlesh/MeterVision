namespace MeterVision.Api.Domain.Exceptions;

/// <summary>
/// Базовое исключение для нарушений бизнес-правил предметной области.
/// </summary>
/// <remarks>
/// Бросается доменными методами (например, <c>Metering.Confirm</c>) при попытке
/// выполнить операцию, недопустимую с точки зрения бизнес-логики.
/// </remarks>
public class DomainException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DomainException"/>
    /// с сообщением по умолчанию.
    /// </summary>
    public DomainException()
        : base("Нарушено бизнес-правило предметной области.")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DomainException"/>
    /// с пользовательским сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public DomainException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DomainException"/>
    /// с пользовательским сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение, ставшее причиной.</param>
    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
