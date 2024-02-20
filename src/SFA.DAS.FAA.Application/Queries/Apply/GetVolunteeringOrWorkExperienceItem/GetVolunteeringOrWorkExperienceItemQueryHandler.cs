using MediatR;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
public class GetVolunteeringOrWorkExperienceItemQueryHandler : IRequestHandler<GetVolunteeringOrWorkExperienceItemQuery, GetVolunteeringOrWorkExperienceItemQueryResult>
{
    private readonly IApiClient _apiClient;
    public GetVolunteeringOrWorkExperienceItemQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetVolunteeringOrWorkExperienceItemQueryResult> Handle(GetVolunteeringOrWorkExperienceItemQuery query, CancellationToken cancellationToken)
    {
        var request = new GetVolunteeringOrWorkExperienceItemApiRequest(query.ApplicationId, query.Id, query.CandidateId);

        var response = await _apiClient.Get<GetVolunteeringOrWorkExperienceItemApiResponse>(request);

        return new GetVolunteeringOrWorkExperienceItemQueryResult
        {
            Id = response.Id,
            ApplicationId = response.ApplicationId,
            Organisation = response.Organisation,
            Description = response.Description,
            StartDate = response.StartDate,
            EndDate = response.EndDate
        };
    }
}
