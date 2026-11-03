namespace MeterVision.Api.Application.Commands.Recognition;

/// <summary>
/// Ответ с результатами распознавания прибора учета
/// </summary>
/// <param name="Value">Распознанные показания счетчика</param>
/// <param name="SerialNumber">Серийный номер счетчика</param>
/// <param name="Confidence">Уровень уверенности распознавания от 0.0 до 1.0</param>
public sealed record RecognitionResponse(string Value, string SerialNumber, double Confidence);
