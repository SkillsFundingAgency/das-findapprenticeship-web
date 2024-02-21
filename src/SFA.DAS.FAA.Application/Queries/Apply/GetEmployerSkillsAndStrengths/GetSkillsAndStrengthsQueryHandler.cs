using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;
public class GetSkillsAndStrengthsQueryHandler(IApiClient ApiClient) : IRequestHandler<GetSkillsAndStrengthsQuery, GetSkillsAndStrengthsQueryResult>
{
    public async Task<GetSkillsAndStrengthsQueryResult> Handle(GetSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        return await ApiClient.Get<GetSkillsAndStrengthsApiResponse>
            (new GetSkillsAndStrengthsApiRequest(request.ApplicationId, request.CandidateId));
    }
}
