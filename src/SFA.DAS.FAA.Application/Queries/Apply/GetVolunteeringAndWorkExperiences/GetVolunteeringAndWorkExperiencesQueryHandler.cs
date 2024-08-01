using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;

public record GetVolunteeringAndWorkExperiencesQueryHandler(IApiClient ApiClient) : IRequestHandler<GetVolunteeringAndWorkExperiencesQuery, GetVolunteeringAndWorkExperiencesQueryResult>
{
    public async Task<GetVolunteeringAndWorkExperiencesQueryResult> Handle(GetVolunteeringAndWorkExperiencesQuery request, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetVolunteeringAndWorkExperiencesApiResponse>(
            new GetVolunteeringAndWorkExperiencesApiRequest(request.ApplicationId, request.CandidateId));

        if (response == null) throw new ResourceNotFoundException();

        return response;
    }
}