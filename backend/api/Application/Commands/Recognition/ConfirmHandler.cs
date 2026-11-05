using Mediator;

namespace MeterVision.Api.Application.Commands.Recognition;

public sealed class ConfirmHandler : IRequestHandler<ConfirmCommand, ConfirmResponse>
{
    public ValueTask<ConfirmResponse> Handle(
        ConfirmCommand request,
        CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(new ConfirmResponse(request.Id));
    }
}
