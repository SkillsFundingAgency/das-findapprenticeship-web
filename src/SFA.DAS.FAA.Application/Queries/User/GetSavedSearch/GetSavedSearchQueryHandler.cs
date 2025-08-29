using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;

public class GetSavedSearchQueryHandler(IApiClient apiClient) : IRequestHandler<GetSavedSearchQuery, GetSavedSearchQueryResult>
{
    public async Task<GetSavedSearchQueryResult> Handle(GetSavedSearchQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetSavedSearchApiResponse>(new GetSavedSearchApiRequest(request.CandidateId, request.Id));
        return result.SavedSearch is null
            ? new GetSavedSearchQueryResult(null, new List<RouteInfo>())
            : new GetSavedSearchQueryResult(
                new SavedSearch(
                    result.SavedSearch.Id,
                    result.SavedSearch.DateCreated,
                    result.SavedSearch.LastRunDate,
                    result.SavedSearch.EmailLastSendDate,
                    new SearchParameters(
                        result.SavedSearch.SearchParameters.SearchTerm,
                        result.SavedSearch.SearchParameters.SelectedRouteIds,
                        result.SavedSearch.SearchParameters.Distance,
                        result.SavedSearch.SearchParameters.DisabilityConfident,
                        result.SavedSearch.SearchParameters.ExcludeNational,
                        result.SavedSearch.SearchParameters.SelectedLevelIds,
                        result.SavedSearch.SearchParameters.Location,
                        result.SavedSearch.SearchParameters.SelectedApprenticeshipTypes
                    )
                ),
                result.Routes.Select(x => new RouteInfo(x.Id, x.Name)).ToList());
    }
}