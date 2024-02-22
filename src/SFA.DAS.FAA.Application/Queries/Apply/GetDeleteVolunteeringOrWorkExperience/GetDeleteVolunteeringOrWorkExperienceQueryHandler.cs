using MediatR;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteVolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceQueryHandler : IRequestHandler<GetDeleteVolunteeringOrWorkExperienceQuery, GetDeleteVolunteeringOrWorkExperienceQueryResult>
{
    private readonly IApiClient _apiClient;
    public GetDeleteVolunteeringOrWorkExperienceQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetDeleteVolunteeringOrWorkExperienceQueryResult> Handle(GetDeleteVolunteeringOrWorkExperienceQuery query, CancellationToken cancellationToken)
    {
        var request = new GetDeleteVolunteeringOrWorkExperienceApiRequest(query.ApplicationId, query.Id, query.CandidateId);

        var response = await _apiClient.Get<GetDeleteVolunteeringOrWorkExperienceApiResponse>(request);

        return new GetDeleteVolunteeringOrWorkExperienceQueryResult
        {
            Id = response.Id,
            ApplicationId = response.ApplicationId,
            Organisation = response.Organisation,
            Description = response.Description,
            FromDate = response.FromDate,
            ToDate = response.ToDate
        };
    }
}
