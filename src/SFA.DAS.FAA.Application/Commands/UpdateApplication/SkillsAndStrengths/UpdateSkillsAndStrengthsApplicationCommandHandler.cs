using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.SkillsAndStrengths;
public class UpdateSkillsAndStrengthsApplicationCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateSkillsAndStrengthsApplicationCommand, UpdateSkillsAndStrengthsApplicationCommandResult>
{
    public async Task<UpdateSkillsAndStrengthsApplicationCommandResult> Handle(UpdateSkillsAndStrengthsApplicationCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateSkillsAndStrengthsApplicationApiRequest(
                    request.ApplicationId,
                    request.CandidateId,
                    new UpdateSkillsAndStrengthsApplicationModel
                    {
                        SkillsAndStrengthsSectionStatus = request.SkillsAndStrengthsSectionStatus
                    });

        var response = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

        return new UpdateSkillsAndStrengthsApplicationCommandResult
        {
            Application = response
        };
    }
}
