using Mediator;
using MeterVision.Api.Application.Abstractions.Services;

namespace MeterVision.Api.Application.Commands.Recognition;

public sealed class ConfirmHandler(
    IMeteringService _meteringService
    ) : IRequestHandler<ConfirmCommand, ConfirmResponse>
{
    public async ValueTask<ConfirmResponse> Handle(
        ConfirmCommand request,
        CancellationToken cancellationToken)
    {
        var metering = await _meteringService.Confirm(request.Id, request.Value, cancellationToken);
        return new ConfirmResponse(metering.Id);
    }
}
