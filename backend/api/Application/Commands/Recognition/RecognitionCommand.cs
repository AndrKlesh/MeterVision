using Mediator;

namespace MeterVision.Api.Application.Commands.Recognition;

/// <summary>
/// Команда распознавания показаний счетчика
/// </summary>
/// <param name="MeterPhoto">Фотография счетчика</param>
public sealed record RecognitionCommand(byte[] MeterPhoto) : IRequest<RecognitionResponse>;
