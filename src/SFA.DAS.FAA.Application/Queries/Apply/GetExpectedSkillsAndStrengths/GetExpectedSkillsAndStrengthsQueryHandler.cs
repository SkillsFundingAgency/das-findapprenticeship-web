using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQueryHandler(IApiClient apiClient) : IRequestHandler<GetExpectedSkillsAndStrengthsQuery, GetExpectedSkillsAndStrengthsQueryResult>
{
    public async Task<GetExpectedSkillsAndStrengthsQueryResult> Handle(GetExpectedSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetExpectedSkillsAndStrengthsApiResponse>
            (new GetExpectedSkillsAndStrengthsApiRequest(request.ApplicationId, request.CandidateId));
    }
}
