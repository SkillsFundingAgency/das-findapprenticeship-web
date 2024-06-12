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

        return new GetCandidateSkillsAndStrengthsQueryResult
        {
            Strengths = response.Strengths,
            ApplicationId = response.ApplicationId
        };
    }
}
