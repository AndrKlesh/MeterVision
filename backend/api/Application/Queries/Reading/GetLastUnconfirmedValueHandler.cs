using Mediator;
using MeterVision.Api.Application.Abstractions.Services;

namespace MeterVision.Api.Application.Queries.Reading;

public sealed class GetLastUnconfirmedValueHandler(
    IMeteringService _meteringService,
    IImageService _imageService)
  : IQueryHandler<GetLastUnconfirmedValueQuery, GetLastUnconfirmedValueResponse>
{
    public async ValueTask<GetLastUnconfirmedValueResponse> Handle(
        GetLastUnconfirmedValueQuery query,
        CancellationToken cancellationToken)
    {
        var unconfirmed = await _meteringService.GetLastUnconfirmed(query.Username, cancellationToken);
        var imageContent = await _imageService.Get(unconfirmed.Id, cancellationToken);
        return new GetLastUnconfirmedValueResponse(
            unconfirmed.Id,
            unconfirmed.MeterSerialNumber,
            unconfirmed.RecognizedValue,
            imageContent);
    }
}
