using MediatR;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.BrowseByInterestsLocation;

public class GetBrowseByInterestsLocationQueryHandler : IRequestHandler<GetBrowseByInterestsLocationQuery,GetBrowseByInterestsLocationQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetBrowseByInterestsLocationQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<GetBrowseByInterestsLocationQueryResult> Handle(GetBrowseByInterestsLocationQuery request, CancellationToken cancellationToken)
    {
        var result =
            await _apiClient.Get<BrowseByInterestsLocationApiResponse>(
                new GetBrowseByInterestsLocationApiRequest(request.LocationSearchTerm));

        return new GetBrowseByInterestsLocationQueryResult
        {
            Location = result.Location
        };
    }
}