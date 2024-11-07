using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetSavedSearches;

public class GetSavedSearchesQueryHandler(IApiClient apiClient) : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
{
    public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetSavedSearchesApiResponse>(new GetSavedSearchesApiRequest(request.CandidateId));

        return new GetSavedSearchesQueryResult
        {
            Routes = result.Routes.Select(x => new RouteInfo(x.Id, x.Name)).ToList(),
            SavedSearches = result.SavedSearches.Select(x => new SavedSearch(
                x.Id,
                x.DateCreated,
                x.LastRunDate,
                x.EmailLastSendDate,
                new SearchParameters(
                    x.SearchParameters.SearchTerm,
                    x.SearchParameters.SelectedRouteIds,
                    x.SearchParameters.Distance,
                    x.SearchParameters.DisabilityConfident,
                    x.SearchParameters.SelectedLevelIds,
                    x.SearchParameters.Location
                )
            )).ToList()
        };
    }
}