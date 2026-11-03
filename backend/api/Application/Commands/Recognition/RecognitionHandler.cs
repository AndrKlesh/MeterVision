using Mediator;
using MeterVision.Api.Services.Abstractions;

namespace MeterVision.Api.Application.Commands.Recognition;

/// <summary>
/// Обработчик команды распознавания показаний
/// </summary>
/// <param name="_cvService">Сервис распознавания показаний</param>
public sealed class RecognitionHandler(ICvService _cvService) : IRequestHandler<RecognitionCommand, RecognitionResponse>
{
    public async ValueTask<RecognitionResponse> Handle(
        RecognitionCommand request,
        CancellationToken cancellationToken)
    {
        return await _cvService.RecognizeAsync(request);
    }
}
