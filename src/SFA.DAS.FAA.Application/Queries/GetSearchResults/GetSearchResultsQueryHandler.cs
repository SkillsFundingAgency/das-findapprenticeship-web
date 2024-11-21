using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetSearchResultsQuery, GetSearchResultsResult>
{
    public async Task<GetSearchResultsResult> Handle(GetSearchResultsQuery query, CancellationToken cancellationToken)
    {
        var vacancySort = VacancySort.DistanceAsc;
        if (!string.IsNullOrEmpty(query.Sort))
        {
            if (Enum.TryParse<VacancySort>(query.Sort, true, out var vacancySortParsed))
            {
                vacancySort = vacancySortParsed;
            }
        }
        
        var request = new GetSearchResultsApiRequest(query.Location,
            query.SelectedRouteIds,
            query.SelectedLevelIds,
            query.Distance,
            query.SearchTerm,
            query.PageNumber,
            query.PageSize,
            vacancySort,
            query.SkipWageType,
            query.DisabilityConfident,
            query.CandidateId);
        var response = await apiClient.Get<GetSearchResultsApiResponse>(request);

        return new GetSearchResultsResult
        {
            Total = response.TotalFound,
            TotalCompetitiveVacanciesCount = response.TotalCompetitiveVacanciesCount,
            Location = response.Location,
            Vacancies = response.Vacancies,
            Routes = response.Routes,
            PageNumber = response.PageNumber,
            TotalPages = response.TotalPages,
            Sort = vacancySort.ToString(),
            SkipWageType = query.SkipWageType?.ToString(),
            VacancyReference = response.VacancyReference,
            Levels = response.Levels,
            DisabilityConfident = response.DisabilityConfident,
            SavedSearchesCount = response.SavedSearchesCount,
            SearchAlreadySaved = response.SearchAlreadySaved
        };
    }
}