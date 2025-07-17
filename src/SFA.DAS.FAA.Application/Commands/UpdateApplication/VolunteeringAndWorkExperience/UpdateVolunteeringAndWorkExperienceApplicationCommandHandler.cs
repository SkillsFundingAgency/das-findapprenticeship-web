using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
public class UpdateVolunteeringAndWorkExperienceApplicationCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateVolunteeringAndWorkExperienceApplicationCommand, UpdateVolunteeringAndWorkExperienceApplicationCommandResult>
{
    public async Task<UpdateVolunteeringAndWorkExperienceApplicationCommandResult> Handle
        (UpdateVolunteeringAndWorkExperienceApplicationCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateVolunteeringAndWorkExperienceApplicationApiRequest(
            request.ApplicationId,
            request.CandidateId,
            new UpdateVolunteeringAndWorkHistoryApplicationModel
            {
                VolunteeringAndWorkExperienceSectionStatus = request.VolunteeringAndWorkExperienceSectionStatus
            });

        var response = await apiClient.Post<Domain.Apply.UpdateApplication.Application>(postRequest);

        return new UpdateVolunteeringAndWorkExperienceApplicationCommandResult
        {
            Application = response
        };
    }
}
