
using MeterVision.Api.Application.Abstractions.Services;

namespace MeterVision.Api.Infrastructure.Services;

/// <summary>
/// Заглушка <see cref="IImageService"/>, сохраняющая файлы в локальную папку.
/// </summary>
/// <remarks>
/// Файлы именуются по <c>{meteringId}.jpg</c>, что исключает path traversal
/// и упрощает поиск. Для production заменить на S3/Azure Blob.
/// <para>
/// TODO: формат фиксирован (JPEG). Если контроллер принимает PNG,
/// файл будет сохранён с расширением <c>.jpg</c>, но содержимое останется PNG.
/// Либо валидировать формат на входе, либо хранить расширение в БД.
/// </para>
/// </remarks>
public class ImageService : IImageService
{
    // TODO: путьзахардкожен — вынести в конфигурацию.
    private readonly string _basePath = Path.Combine(
        Directory.GetCurrentDirectory(), "uploads");

    /// <summary>
    /// Сохраняет фотографию для указанного показания.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="content">Содержимое файла (JPEG).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    public async Task Save(Guid meteringId, byte[] content, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_basePath);

        var fullPath = Path.Combine(_basePath, $"{meteringId:N}.jpg");
        await File.WriteAllBytesAsync(fullPath, content, cancellationToken);
    }

    /// <summary>
    /// Извлекает фотографию для указанного показания.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Содержимое файла (JPEG).</returns>
    /// <exception cref="FileNotFoundException">
    /// Файл для указанного показания не существует.
    /// </exception>
    public async Task<byte[]> Get(Guid meteringId, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_basePath, $"{meteringId:N}.jpg");

        if (!File.Exists(fullPath))
            throw new FileNotFoundException(
                $"Файл для показания {meteringId} не найден.", fullPath);

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }
}
