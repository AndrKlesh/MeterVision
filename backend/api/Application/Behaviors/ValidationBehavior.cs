using FluentValidation;
using Mediator;

namespace MeterVision.Api.Application.Behaviors;
/// <summary>
/// Сквозной Behavior конвейера Mediator, выполняющий валидацию запроса
/// перед передачей управления обработчику.
/// </summary>
/// <typeparam name="TRequest">Тип обрабатываемого запроса (команды или запроса).</typeparam>
/// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
/// <remarks>
/// Behavior является обобщённым и применяется ко всем запросам, для которых
/// в DI-контейнере зарегистрированы реализации <see cref="IValidator{T}"/>.
/// Если валидаторы для <typeparamref name="TRequest"/> отсутствуют, запрос
/// передаётся следующему делегату без проверок.
/// <para>
/// При обнаружении ошибок валидации выбрасывается <see cref="ValidationException"/>
/// со списком всех найденных нарушений. Исключение должно быть обработано
/// глобальным обработчиком ошибок (middleware) и преобразовано в HTTP 400.
/// </para>
/// <para>
/// Регистрируется в <c>Program.cs</c> через
/// <c>options.PipelineBehaviors = [typeof(ValidationBehavior&lt;,&gt;)]</c>.
/// Валидаторы подключаются через <c>AddValidatorsFromAssemblyContaining</c>.
/// </para>
/// </remarks>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ValidationBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="validators">
    /// Коллекция валидаторов, применимых к <typeparamref name="TRequest"/>.
    /// Может быть пустой, если для запроса не зарегистрировано ни одного валидатора.
    /// </param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    /// <summary>
    /// Выполняет валидацию запроса и, при успехе, передаёт его следующему
    /// делегату в конвейере обработки.
    /// </summary>
    /// <param name="request">Экземпляр запроса, подлежащий валидации.</param>
    /// <param name="next">Следующий делегат конвейера (обработчик или другой Behavior).</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса следующим делегатом.</returns>
    /// <exception cref="ValidationException">
    /// Выбрасывается, если хотя бы один валидатор вернул ошибки.
    /// </exception>
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken ct)
    {
        // Если для данного типа запроса нет валидаторов — пропускаем без проверок.
        // Это позволяет использовать Behavior глобально, не создавая пустые валидаторы.
        if (!_validators.Any())
            return await next(request, ct);

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, ct)));

        // Собираем все ошибки от всех валидаторов в один список.
        // Это позволяет вернуть клиенту полную картину нарушений, а не только первое.
        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(request, ct);
    }
}
