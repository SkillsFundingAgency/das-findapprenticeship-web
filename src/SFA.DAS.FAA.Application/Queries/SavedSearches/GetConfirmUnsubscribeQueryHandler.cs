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
        var request = new GetConfirmUnsubscribeApiRequest(query.SearchTerm);
        var response = await _apiClient.Get<ConfirmUnsubscribeApiResponse>(request);
        return new GetConfirmUnsubscribeResult
        {
            Where = response.Where,
            Distance = response.Distance,
            Categories = response.Categories,
            Levels = response.Levels
        };
    }
}

