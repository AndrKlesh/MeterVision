using Mediator;

namespace MeterVision.Api.Application.Commands.Recognition;

public sealed record ConfirmCommand(int Id, string SerialNumber, double Value)
    : IRequest<ConfirmResponse>;
