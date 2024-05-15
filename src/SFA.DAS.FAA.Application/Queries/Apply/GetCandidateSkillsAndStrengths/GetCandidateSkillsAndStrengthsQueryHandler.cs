using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQueryHandler(IApiClient apiClient) 
    : IRequestHandler<GetCandidateSkillsAndStrengthsQuery, GetCandidateSkillsAndStrengthsQueryResult>
{
    public async Task<GetCandidateSkillsAndStrengthsQueryResult> Handle(GetCandidateSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetCandidateSkillsAndStrengthsApiResponse>
            (new GetCandidateSkillsAndStrengthsApiRequest(request.CandidateId, request.ApplicationId));

        if (response.AboutYou == null) return new GetCandidateSkillsAndStrengthsQueryResult();

        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            SkillsAndStrengths = response.AboutYou.SkillsAndStrengths,
            Support = response.AboutYou.Support,
            ApplicationId = response.AboutYou.ApplicationId
        };
    }
}
