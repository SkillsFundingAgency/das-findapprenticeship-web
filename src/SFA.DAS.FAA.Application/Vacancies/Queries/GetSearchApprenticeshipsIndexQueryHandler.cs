using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Application.Vacancies.Queries;

public class GetSearchApprenticeshipsIndexQueryHandler : IRequestHandler<GetSearchApprenticeshipsIndexQuery, GetSearchApprenticeshipsIndexResult>
{
    private readonly IApiClient _apiClient;

    public GetSearchApprenticeshipsIndexQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetSearchApprenticeshipsIndexResult> Handle(GetSearchApprenticeshipsIndexQuery query, CancellationToken cancellationToken)
    {
        var request = new GetSearchApprenticeshipsIndexApiRequest();
        var response = await _apiClient.Get<SearchApprenticeshipsApiResponse>(request);
        return new GetSearchApprenticeshipsIndexResult
        {
            Total = response.Total,
        };
    }
}