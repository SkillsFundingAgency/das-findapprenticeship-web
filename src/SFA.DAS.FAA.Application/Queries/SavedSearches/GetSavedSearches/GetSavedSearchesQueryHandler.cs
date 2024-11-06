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
            SavedSearches = result.SavedSearches.Select(x => new SavedSearch(
                x.Id,
                x.DateCreated,
                x.LastRunDate,
                x.EmailLastSendDate,
                new SearchParameters(
                    x.SearchParameters.SearchTerm,
                    x.SearchParameters.Categories,
                    x.SearchParameters.Distance,
                    x.SearchParameters.DisabilityConfident,
                    x.SearchParameters.Levels,
                    x.SearchParameters.Location
                )
            )).ToList()
        };
    }
}