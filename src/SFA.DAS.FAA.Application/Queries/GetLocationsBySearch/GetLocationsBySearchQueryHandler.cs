using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.LocationsBySearch;

namespace SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
public class GetLocationsBySearchQueryHandler : IRequestHandler<GetLocationsBySearchQuery, GetLocationsBySearchQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetLocationsBySearchQueryHandler(IApiClient apiClient) => _apiClient = apiClient;

    public async Task<GetLocationsBySearchQueryResult> Handle(GetLocationsBySearchQuery query, CancellationToken cancellationToken)
    {
        var request = new GetLocationsBySearchApiRequest(query.SearchTerm);
        var results = await _apiClient.Get<GetLocationsBySearchApiResponse>(request);

        return new GetLocationsBySearchQueryResult
        {
            LocationItems = results.LocationItems
        };
    }
}