using MediatR;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;

public record UpdateVolunteeringAndWorkExperienceCommandHandler : IRequestHandler<UpdateVolunteeringAndWorkExperienceCommand>
{
    private readonly IApiClient _apiClient;

    public UpdateVolunteeringAndWorkExperienceCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task Handle(UpdateVolunteeringAndWorkExperienceCommand request, CancellationToken cancellationToken)
    {
        var data = new PostUpdateVolunteeringOrWorkExperienceApiRequest.PostUpdateVolunteeringOrWorkExperienceApiRequestData
        {
            CandidateId = request.CandidateId,
            EmployerName = request.CompanyName,
            EndDate = request.EndDate,
            StartDate = request.StartDate,
            Description = request.Description
        };

        var apiRequest = new PostUpdateVolunteeringOrWorkExperienceApiRequest(request.ApplicationId, request.VolunteeringOrWorkExperienceId, data);

        await _apiClient.PostWithResponseCode(apiRequest);
    }
}