using MediatR;
using SFA.DAS.FAA.Domain.Apply.CreateSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
public class CreateSkillsAndStrengthsCommandHandler(IApiClient apiClient) : IRequestHandler<CreateSkillsAndStrengthsCommand>
{
    public async Task Handle(CreateSkillsAndStrengthsCommand request, CancellationToken cancellationToken)
    {
        var data = new PostSkillsAndStrengthsApiRequest.PostCreateSkillsAndStrengthsRequestData
        {
            CandidateId = request.CandidateId,
            SkillsAndStrengths = request.SkillsAndStrengths
        };

        var apiRequest = new PostSkillsAndStrengthsApiRequest(request.ApplicationId, data);

        await apiClient.PostWithResponseCode(apiRequest);
    }
}
