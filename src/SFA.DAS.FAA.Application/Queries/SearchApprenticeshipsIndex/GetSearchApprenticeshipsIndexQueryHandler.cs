using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexQueryHandler : IRequestHandler<GetSearchApprenticeshipsIndexQuery, GetSearchApprenticeshipsIndexResult>
{
    private readonly IApiClient _apiClient;

    public GetSearchApprenticeshipsIndexQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetSearchApprenticeshipsIndexResult> Handle(GetSearchApprenticeshipsIndexQuery query, CancellationToken cancellationToken)
    {
        var request = new GetSearchApprenticeshipsIndexApiRequest(query.LocationSearchTerm);
        var response = await _apiClient.Get<SearchApprenticeshipsApiResponse>(request);
        return new GetSearchApprenticeshipsIndexResult
        {
            Total = response.Total,
            LocationSearched = response.LocationSearched,
            Location = response.Location
        };
    }
}