using MediatR;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Vacancies.Queries;

public class GetSearchApprenticeshipsIndexQueryHandler : IRequestHandler<GetSearchApprenticeshipsIndexQuery, GetSearchApprenticeshipsIndexResult>
{
    private readonly IApiClient _apiClient;
    private readonly FindAnApprenticeshipApi _config;

    public GetSearchApprenticeshipsIndexQueryHandler(IApiClient apiClient, FindAnApprenticeshipApi config)
    {
        _apiClient = apiClient;
        _config = config;
    }
    public async Task<GetSearchApprenticeshipsIndexResult> Handle(GetSearchApprenticeshipsIndexQuery query, CancellationToken cancellationToken)
    {
        var request = new GetSearchApprenticeshipsIndexApiRequest();
        var response = await _apiClient.Get<SearchApprenticeshipsIndex>(request);
        return new GetSearchApprenticeshipsIndexResult
        {
            Total = response.Total,
        };
    }
}