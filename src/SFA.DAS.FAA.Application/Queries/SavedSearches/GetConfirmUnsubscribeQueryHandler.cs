using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches;

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
        var response = await _apiClient.Get<ConfirmSavedSearchUnsubscribeApiResponse>(request);
        return new GetConfirmUnsubscribeResult
        {
            Where = response.Where,
            Distance = response.Distance,
            Categories = response.Categories,
            Levels = response.Levels
        };
    }
}

