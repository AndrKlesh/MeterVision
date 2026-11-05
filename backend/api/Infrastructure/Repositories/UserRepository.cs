using MeterVision.Api.Application.Abstractions.Repositories;
using MeterVision.Api.Application.Abstractions.Repositories.Exceptions;
using MeterVision.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

using User = MeterVision.Api.Domain.Entities.Auth.User;
using DbUser = MeterVision.Api.Infrastructure.Database.Models.User;


namespace MeterVision.Api.Infrastructure.Repositories;

/// <summary>
/// Реализация <see cref="IUserRepository"/> поверх EF Core и <see cref="AppDatabase"/>.
/// </summary>
/// <remarks>
/// Работает с технической моделью БД <see cref="User"/> (Infrastructure.Database.Models),
/// а не с доменной сущностью. Если в будущем появится доменный <c>User</c> в Domain,
/// потребуется маппинг между слоями.
/// <para>
/// Регистрируется в DI как <c>Scoped</c> (см. <c>AddInfrastructure</c>).
/// </para>
/// </remarks>
/// <remarks>
/// Инициализирует новый экземпляр <see cref="UserRepository"/>.
/// </remarks>
/// <param name="db">Контекст базы данных, внедряемый через DI.</param>
public class UserRepository(AppDatabase db) : IUserRepository
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
    public async Task<User> Get(int id, CancellationToken cancellationToken)
    {
        // TODO: Id в User — Guid, а метод принимает int.
        // Пока используем ToString() как костыль; нужно привести типы к единому виду.
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id.ToString()), cancellationToken);

        if (user is null)
            throw new UserNotFoundException($"Пользователь с Id={id} не найден.");

        var idFromDb = int.Parse(user.Id.ToString().Replace("-", "").Substring(0, 8),
            System.Globalization.NumberStyles.HexNumber);
        return new User(idFromDb, user.Username, user.PasswordHash);
    }

    /// <summary>
    /// Возвращает пользователя по имени (логину).
    /// </summary>
    /// <param name="username">Имя пользователя (логин).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Найденный пользователь.</returns>
    /// <exception cref="UserNotFoundException">
    /// Пользователь с указанным именем не существует.
    /// </exception>
    public async Task<User> Get(string username, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
            throw new UserNotFoundException($"Пользователь с именем '{username}' не найден.");

        var idFromDb = int.Parse(user.Id.ToString().Replace("-", "").Substring(0, 8),
            System.Globalization.NumberStyles.HexNumber);
        return new User(idFromDb, user.Username, user.PasswordHash);
    }

    /// <summary>
    /// Сохраняет пользователя (создаёт нового или обновляет существующего).
    /// </summary>
    /// <param name="user">Экземпляр пользователя для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Идентификатор сохранённого пользователя (в виде int).</returns>
    /// <exception cref="DuplicateUsernameException">
    /// Пользователь с таким именем уже существует (нарушение уникальности).
    /// </exception>
    /// <remarks>
    /// Метод используется и для создания, и для обновления:
    /// если <c>user.Id == Guid.Empty</c> — создаётся новый пользователь,
    /// иначе — обновляется существующий.
    /// <para>
    /// TODO: разделить на <c>Add</c> и <c>Update</c>; сейчас неоднозначно.
    /// </para>
    /// </remarks>
    public async Task<int> Put(User user, CancellationToken cancellationToken)
    {
        var existing = await _db.Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == user.Id.ToString(), cancellationToken);

        if (existing is null)
        {
            // Создание: проверяем уникальность Username
            var usernameTaken = await _db.Users
                .AnyAsync(u => u.Username == user.Username, cancellationToken);

            if (usernameTaken)
                throw new DuplicateUsernameException($"Имя '{user.Username}' уже занято.");

            // TODO: Id в User — Guid
            var guid = new Guid(user.Id, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            _db.Users.Add(new DbUser() { Id = guid, Username = user.Username, PasswordHash = user.PasswordHash });
        }
        else
        {
            // Обновление: проверяем, что новый Username не занят другим пользователем
            var usernameTaken = await _db.Users
                .AnyAsync(u => u.Username == user.Username && u.Id.ToString() != user.Id.ToString(), cancellationToken);

            if (usernameTaken)
                throw new DuplicateUsernameException($"Имя '{user.Username}' уже занято.");

            existing.Username = user.Username;
            existing.PasswordHash = user.PasswordHash;
        }

        await _db.SaveChangesAsync(cancellationToken);

        // TODO: Id в User — Guid, а метод возвращает int. Это костыль:
        // либо менять тип Id в IUserRepository на Guid, либо хранить числовой ключ.
        return int.Parse(user.Id.ToString().Replace("-", "").Substring(0, 8),
            System.Globalization.NumberStyles.HexNumber);
    }

    private readonly AppDatabase _db = db;
}