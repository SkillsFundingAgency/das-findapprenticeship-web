using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;

public class GetConfirmUnsubscribeQueryHandler : IRequestHandler<GetConfirmUnsubscribeQuery, GetConfirmUnsubscribeResult>
{
    private readonly IApiClient _apiClient;

    public GetConfirmUnsubscribeQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetConfirmUnsubscribeResult> Handle(GetConfirmUnsubscribeQuery query, CancellationToken cancellationToken)
    {
        var request = new GetConfirmSavedSearchUnsubscribeApiRequest(query.SavedSearchId);
        var response = await _apiClient.Get<ConfirmSavedSearchUnsubscribeApiResponse?>(request);
        if (response == null)
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
                    response.SavedSearch.SearchParameters.SelectedLevelIds,
                    response.SavedSearch.SearchParameters.Location
                ))
        };
    }
}

