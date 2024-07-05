using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQueryHandler(IApiClient apiClient) : IRequestHandler<GetExpectedSkillsAndStrengthsQuery, GetExpectedSkillsAndStrengthsQueryResult>
{
    public async Task<GetExpectedSkillsAndStrengthsQueryResult> Handle(GetExpectedSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetExpectedSkillsAndStrengthsApiResponse>
            (new GetExpectedSkillsAndStrengthsApiRequest(request.ApplicationId, request.CandidateId));

        if (response == null) throw new ResourceNotFoundException();

        return response;
    }
}
