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
        var sort = string.IsNullOrEmpty(query.Sort)
            ? VacancySort.DistanceAsc
            : (VacancySort)Enum.Parse(typeof(VacancySort), query.Sort, true);

        var request = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.SelectedLevelIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize, sort, query.DisabilityConfident);
        var response = await _apiClient.Get<GetSearchResultsApiResponse>(request);

        return new GetSearchResultsResult
        {
            Total = response.TotalFound,
            Location = response.Location,
            Vacancies = response.Vacancies,
            Routes = response.Routes,
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalPages = response.TotalPages,
            Sort = sort.ToString(),
            VacancyReference = response.VacancyReference,
            Levels = response.Levels,
            DisabilityConfident = response.DisabilityConfident,
        };
    }
}