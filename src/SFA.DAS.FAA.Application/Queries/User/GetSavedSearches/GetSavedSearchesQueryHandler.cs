using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;

public class GetSavedSearchesQueryHandler(IApiClient apiClient) : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
{
    public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetSavedSearchesApiResponse>(new GetSavedSearchesApiRequest(request.CandidateId));

        return new GetSavedSearchesQueryResult(
            result.SavedSearches.Select(x => new SavedSearch(
                x.Id,
                x.DateCreated,
                x.LastRunDate,
                x.EmailLastSendDate,
                new SearchParameters(
                    x.SearchParameters.SearchTerm,
                    x.SearchParameters.SelectedRouteIds,
                    x.SearchParameters.Distance,
                    x.SearchParameters.DisabilityConfident,
                    x.SearchParameters.ExcludeNational,
                    x.SearchParameters.SelectedLevelIds,
                    x.SearchParameters.Location,
                    x.SearchParameters.SelectedApprenticeshipTypes
                )
            )).ToList(),
            result.Routes.Select(x => new RouteInfo(x.Id, x.Name)).ToList());
    }
}