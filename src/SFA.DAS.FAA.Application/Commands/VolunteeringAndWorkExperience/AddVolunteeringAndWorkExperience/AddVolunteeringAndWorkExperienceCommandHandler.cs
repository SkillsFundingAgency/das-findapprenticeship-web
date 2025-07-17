using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;

public record AddVolunteeringAndWorkExperienceCommandHandler : IRequestHandler<AddVolunteeringAndWorkExperienceCommand, AddVolunteeringAndWorkExperienceCommandResult>
{
    private readonly IApiClient _apiClient;

    public AddVolunteeringAndWorkExperienceCommandHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<AddVolunteeringAndWorkExperienceCommandResult> Handle(AddVolunteeringAndWorkExperienceCommand request, CancellationToken cancellationToken)
    {
        var data = new PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData
        {
            CandidateId = request.CandidateId,
            CompanyName = request.CompanyName,
            EndDate = request.EndDate,
            StartDate = request.StartDate,
            Description = request.Description
        };

        var apiRequest = new PostVolunteeringAndWorkExperienceRequest(request.ApplicationId, data);

        var apiResponse = await _apiClient.Post<PostVolunteeringAndWorkExperienceResponse>(apiRequest);

        if (apiResponse != null)
            return new AddVolunteeringAndWorkExperienceCommandResult
            {
                Id = apiResponse.Id
            };

        return new AddVolunteeringAndWorkExperienceCommandResult();
    }
}