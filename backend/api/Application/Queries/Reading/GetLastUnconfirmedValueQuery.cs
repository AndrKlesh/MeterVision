using Mediator;

namespace MeterVision.Api.Application.Queries.Reading;

public sealed record GetLastUnconfirmedValueQuery(string Username)
    : IQuery<GetLastUnconfirmedValueResponse>;
