using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;

public class GetConfirmUnsubscribeQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetConfirmUnsubscribeQuery, GetConfirmUnsubscribeResult>
{
    public async Task<GetConfirmUnsubscribeResult> Handle(GetConfirmUnsubscribeQuery query, CancellationToken cancellationToken)
    {
        var request = new GetConfirmSavedSearchUnsubscribeApiRequest(query.SavedSearchId);
        var response = await apiClient.Get<ConfirmSavedSearchUnsubscribeApiResponse?>(request);
        if (response?.SavedSearch == null)
        {
            return new GetConfirmUnsubscribeResult();
        }

        return new GetConfirmUnsubscribeResult
        {
            Routes = response.Routes.Select(c=>new RouteInfo(c.Id,c.Name)).ToList(),
            SavedSearch = new SavedSearch(
                response.SavedSearch.Id,
                response.SavedSearch.DateCreated,
                response.SavedSearch.LastRunDate,
                response.SavedSearch.EmailLastSendDate,
                new SearchParameters(
                    response.SavedSearch.SearchParameters.SearchTerm,
                    response.SavedSearch.SearchParameters.SelectedRouteIds,
                    response.SavedSearch.SearchParameters.Distance,
                    response.SavedSearch.SearchParameters.DisabilityConfident,
                    response.SavedSearch.SearchParameters.ExcludeNational,
                    response.SavedSearch.SearchParameters.SelectedLevelIds,
                    response.SavedSearch.SearchParameters.Location,
                    response.SavedSearch.SearchParameters.SelectedApprenticeshipTypes
                ))
        };
    }
}

