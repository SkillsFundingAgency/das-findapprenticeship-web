using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsQueryHandler : IRequestHandler<GetSearchResultsQuery, GetSearchResultsResult>
{
    private readonly IApiClient _apiClient;

    public GetSearchResultsQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetSearchResultsResult> Handle(GetSearchResultsQuery query, CancellationToken cancellationToken)
    {
        var request = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize);
        var response = await _apiClient.Get<GetSearchResultsApiResponse>(request);
        return new GetSearchResultsResult
        {
            Total = response.Total,
            Location = response.Location,
            Vacancies = response.Vacancies,
            Routes = response.Routes,
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalPages = response.TotalPages,
        };
    }
}