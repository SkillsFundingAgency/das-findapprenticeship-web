using MediatR;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class DeleteVolunteeringOrWorkExperienceCommandHandler : IRequestHandler<DeleteVolunteeringOrWorkExperienceCommand, Unit>
{
    private readonly IApiClient _apiClient;

    public DeleteVolunteeringOrWorkExperienceCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(DeleteVolunteeringOrWorkExperienceCommand command, CancellationToken cancellationToken)
    {
        var request = new PostDeleteVolunteeringOrWorkExperienceApiRequest(command.ApplicationId, command.Id, new PostDeleteVolunteeringOrWorkExperienceApiRequest.PostDeleteVolunteeringOrWorkExperienceApiRequestData
        {
            CandidateId = command.CandidateId,
        });

        await _apiClient.Post(request);
        return Unit.Value;
    }
}
