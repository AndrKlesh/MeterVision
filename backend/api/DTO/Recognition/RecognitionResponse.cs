namespace MeterVision.Api.DTO.Recognition;

/// <summary>
/// Ответ с результатами распознавания прибора учета
/// </summary>
public class RecognitionResponse
{
    /// <summary>
    /// Распознанные показания счетчика (например, "50.99")
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Серийный номер счетчика (например, "51152054")
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Уровень уверенности распознавания от 0.0 до 1.0
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Ссылка или путь к сохраненной фотографии на сервере
    /// </summary>
    public string PhotoUrl { get; set; } = string.Empty;
}