using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQueryHandler(IApiClient ApiClient) : IRequestHandler<GetExpectedSkillsAndStrengthsQuery, GetExpectedSkillsAndStrengthsQueryResult>
{
    public async Task<GetExpectedSkillsAndStrengthsQueryResult> Handle(GetExpectedSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        return await ApiClient.Get<GetExpectedSkillsAndStrengthsApiResponse>
            (new GetExpectedSkillsAndStrengthsApiRequest(request.ApplicationId, request.CandidateId));
    }
}
