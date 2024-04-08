using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateName;
public class GetCandidateNameQueryHandler : IRequestHandler<GetCandidateNameQuery, GetCandidateNameQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidateNameQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidateNameQueryResult> Handle(GetCandidateNameQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.Get<GetCandidateNameApiResponse>(new GetCandidateNameApiRequest(request.CandidateId));
    }
}
