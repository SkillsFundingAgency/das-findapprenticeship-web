using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;


namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
public class GetLocationsByIndexSearchQueryHandler : IRequestHandler<GetLocationsByIndexSearchQuery, GetLocationsByIndexSearchQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetLocationsByIndexSearchQueryHandler(IApiClient apiClient) => _apiClient = apiClient;

    public async Task<GetLocationsByIndexSearchQueryResult> Handle(GetLocationsByIndexSearchQuery query, CancellationToken cancellationToken)
    {
        var request = new GetLocationsByIndexSearchApiRequest(query.SearchTerm);
        var results = await _apiClient.Get<GetLocationsByIndexSearchApiResponse>(request);

        return new GetLocationsByIndexSearchQueryResult
        {
            LocationItems = results.LocationItems
        };
    }
}