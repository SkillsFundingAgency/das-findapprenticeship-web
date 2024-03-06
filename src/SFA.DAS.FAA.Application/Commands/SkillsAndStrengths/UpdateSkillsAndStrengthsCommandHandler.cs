using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
public class UpdateSkillsAndStrengthsCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateSkillsAndStrengthsCommand, UpdateSkillsAndStrengthsCommandResult>
{
    public async Task<UpdateSkillsAndStrengthsCommandResult> Handle(UpdateSkillsAndStrengthsCommand request, CancellationToken cancellationToken)
    {
        var postUpdateApplicationRequest = new PostSkillsAndStrengthsApiRequest(
                    request.ApplicationId,
                    new PostSkillsAndStrengthsModel
                    {
                        CandidateId = request.CandidateId,
                        SkillsAndStrengths = request.SkillsAndStrengths,
                        SkillsAndStrengthsSectionStatus = request.SkillsAndStrengthsSectionStatus
                    });

        var updateApplicationResponse = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postUpdateApplicationRequest);

        return new UpdateSkillsAndStrengthsCommandResult
        {
            Application = updateApplicationResponse
        };
    }
}
