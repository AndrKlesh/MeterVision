using MeterVision.Api.Domain.Entities.Auth;
using MeterVision.Api.Application.Abstractions.Exceptions;

namespace MeterVision.Api.Application.Abstractions.Repositories;

/// <summary>
/// Контракт репозитория для работы с пользователями.
/// </summary>
/// <remarks>
/// Определяет операции чтения и сохранения доменной сущности <see cref="User"/>.
/// <para>
/// Интерфейс использует модель исключений для сигнализации об ошибках.
/// </para>
/// <para>
/// Исключения, которые может бросать реализация:
/// <list type="bullet">
///   <item><see cref="UserNotFoundException"/> — пользователь не найден.</item>
///   <item><see cref="DuplicateUsernameException"/> — нарушение уникальности имени.</item>
/// </list>
/// </para>
/// </remarks>
public interface IUserRepository
{
    /// <summary>
    /// Возвращает пользователя по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Найденный пользователь.</returns>
    /// <exception cref="UserNotFoundException">
    /// Пользователь с указанным идентификатором не существует.
    /// </exception>
    /// <exception cref="InfrastructureException">
    /// Ошибка при обращении к хранилищу (например, БД недоступна).
    /// </exception>
    public Task<User> Get(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает пользователя по имени (логину).
    /// </summary>
    /// <param name="username">Имя пользователя (логин).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Найденный пользователь.</returns>
    /// <exception cref="UserNotFoundException">
    /// Пользователь с указанным именем не существует.
    /// </exception>
    /// <remarks>
    /// Используется в сценарии аутентификации для проверки учётных данных.
    /// </remarks>
    public Task<User> Get(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Сохраняет пользователя (создаёт нового или обновляет существующего).
    /// </summary>
    /// <param name="user">Экземпляр пользователя для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Идентификатор сохранённого пользователя.</returns>
    /// <exception cref="DuplicateUsernameException">
    /// Пользователь с таким именем уже существует (нарушение уникальности).
    /// <remarks>
    /// Метод используется и для создания, и для обновления:
    /// если <c>user.Id == 0</c> — создаётся новый пользователь,
    /// иначе — обновляется существующий.
    /// <para>
    /// Возвращаемый идентификатор позволяет вызывающему коду использовать
    /// его без повторного запроса к базе данных.
    /// </para>
    /// TODO: разделить на <c>Add</c> и <c>Update</c> для явности сценариев
    /// и убрать неоднозначность.
    /// </remarks>
    public Task<int> Put(User user, CancellationToken cancellationToken);
}
