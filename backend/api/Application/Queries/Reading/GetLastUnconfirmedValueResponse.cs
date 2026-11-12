namespace MeterVision.Api.Application.Queries.Reading;

public sealed record GetLastUnconfirmedValueResponse(Guid Id, string SerialNumber, double Value, byte[] ImageContent);
