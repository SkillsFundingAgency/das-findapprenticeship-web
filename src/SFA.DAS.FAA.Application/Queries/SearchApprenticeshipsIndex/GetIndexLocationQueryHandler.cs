using MediatR;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

public class GetIndexLocationQueryHandler : IRequestHandler<GetIndexLocationQuery,GetIndexLocationQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetIndexLocationQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<GetIndexLocationQueryResult> Handle(GetIndexLocationQuery request, CancellationToken cancellationToken)
    {
        var result =
            await _apiClient.Get<IndexLocationApiResponse>(
                new IndexLocationApiRequest(request.LocationSearchTerm));

        return new GetIndexLocationQueryResult
        {
            Location = result.Location
        };
    }
}