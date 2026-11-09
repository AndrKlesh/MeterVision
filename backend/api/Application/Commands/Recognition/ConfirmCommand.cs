using Mediator;

namespace MeterVision.Api.Application.Commands.Recognition;

public sealed record ConfirmCommand(Guid Id, string SerialNumber, double Value)
    : IRequest<ConfirmResponse>;
