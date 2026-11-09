namespace MeterVision.Api.Application.Abstractions.Services;

/// <summary>
/// Сервис для сохранения и извлечения фотографий счётчиков.
/// </summary>
/// <remarks>
/// Абстракция над файловым хранилищем. Файлы адресуются по идентификатору
/// показания (<see cref="Guid"/>), что упрощает
/// связь «показание → файл».
/// <para>
/// Формат файла фиксирован (JPEG). 
/// TODO: Если в будущем потребуется поддержка
/// других форматов, добавить параметр <c>extension</c> или поле в модель.
/// </para>
/// <para>
/// TODO: рассмотреть потоковый API (<c>Stream</c>) вместо <c>byte[]</c>,
/// чтобы не грузить большие файлы в память целиком.
/// </para>
/// </remarks>
public interface IImageService
{
    /// <summary>
    /// Сохраняет фотографию для указанного показания.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания, к которому относится файл.</param>
    /// <param name="content">Содержимое файла (JPEG).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    public Task Save(Guid meteringId, byte[] content, CancellationToken cancellationToken);

    /// <summary>
    /// Извлекает фотографию для указанного показания.
    /// </summary>
    /// <param name="meteringId">Идентификатор показания.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Содержимое файла (JPEG).</returns>
    /// <exception cref="FileNotFoundException">
    /// Файл для указанного показания не существует.
    /// </exception>
    public Task<byte[]> Get(Guid meteringId, CancellationToken cancellationToken);
}
