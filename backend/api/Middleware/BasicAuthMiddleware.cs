
using System.Text;
using MeterVision.Api.Application.Abstractions.Services;

namespace MeterVision.Api.Middleware;

/// <summary>
/// Middleware для проверки аутентификации Basic Auth.
/// Применяется только к путям <c>/api/*</c>, кроме <c>/api/auth/*</c>.
/// </summary>
/// <remarks>
/// Ожидает заголовок <c>Authorization: Basic &lt;base64(username:password)&gt;</c>.
/// При успешной проверке через <see cref="IAuthService.LoginAsync"/> пропускает запрос дальше,
/// иначе возвращает <c>401 Unauthorized</c>.
/// </remarks>
public class BasicAuthMiddleware
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BasicAuthMiddleware"/>.
    /// </summary>
    /// <param name="next">Следующий делегат в конвейере обработки запроса.</param>
    public BasicAuthMiddleware(RequestDelegate next) => _next = next;

    /// <summary>
    /// Обрабатывает HTTP-запрос: проверяет Basic Auth для защищённых путей.
    /// </summary>
    /// <param name="context">Контекст текущего HTTP-запроса.</param>
    /// <param name="authService">Сервис аутентификации для проверки учётных данных.</param>
    /// <param name="logger">Логгер для записи этапов аутентификации.</param>
    /// <returns>Задача, представляющая асинхронную обработку запроса.</returns>
    /// <remarks>
    /// Пути вне <c>/api</c> и путь <c>/api/auth</c> пропускаются без проверки.
    /// При отсутствии или неверном заголовке возвращается <c>401</c>.
    /// </remarks>
    public async Task InvokeAsync(
        HttpContext context,
        IAuthService authService,
        ILogger<BasicAuthMiddleware> logger)
    {
        var path = context.Request.Path;
        if (!path.StartsWithSegments("/api") || path.StartsWithSegments("/api/auth"))
        {
            await _next(context);
            return;
        }

        var header = context.Request.Headers["Authorization"].ToString();
        if (header.StartsWith("Basic "))
        {
            var decoded = Encoding.UTF8.GetString(
                Convert.FromBase64String(header.Substring(6)));

            var parts = decoded.Split(':');
            logger.LogInformation("Начало аутентификации (Username:'{username}' Password:{password})", parts[0], parts[1]);

            try
            {
                var token = await authService.LoginAsync(parts[0], parts[1]);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await _next(context);
                    logger.LogInformation("Аутентификация пройдена (Username:'{username}' Password:{password})", parts[0], parts[1]);
                    return;
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(e, "Сбой аутентификации (Username:'{username}' Password:{password})", parts[0], parts[1]);
            }

            logger.LogInformation("Аутентификация не пройдена (Username:'{username}' Password:{password})", parts[0], parts[1]);
        }

        context.Response.StatusCode = 401;
    }

    private readonly RequestDelegate _next;
}
