using MediatR;
using SFA.DAS.FAA.Domain.GeoPoint;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.GetGeoPoint;
public class GetGeoPointQueryHandler : IRequestHandler<GetGeoPointQuery, GetGeoPointQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetGeoPointQueryHandler(IApiClient apiClient) => _apiClient = apiClient;

    public async Task<GetGeoPointQueryResult> Handle(GetGeoPointQuery query, CancellationToken cancellationToken)
    {
        var request = new GetGeoPointApiRequest(query.PostCode);
        var result = await _apiClient.Get<GetGeoPointApiResponse>(request);

        return new GetGeoPointQueryResult()
        {
            PostCode = result.GeoPoint.Postcode,
            Latitude = result.GeoPoint.Latitude,
            Longitude = result.GeoPoint.Longitude
        };
    }
}