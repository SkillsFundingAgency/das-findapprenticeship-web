using MediatR;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.BrowseByInterests;

public class GetBrowseByInterestsQueryHandler : IRequestHandler<GetBrowseByInterestsQuery, GetBrowseByInterestsResult>
{
    private readonly IApiClient _apiClient;

    public GetBrowseByInterestsQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetBrowseByInterestsResult> Handle(GetBrowseByInterestsQuery query, CancellationToken cancellationToken)
    {
        var request = new GetBrowseByInterestsApiRequest();
        var response = await _apiClient.Get<BrowseByInterestsApiResponse>(request);
        return new GetBrowseByInterestsResult()
        {
            Routes = response.Routes,
        };
    }
}