using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;

public record GetVolunteeringAndWorkExperiencesQuery : IRequest<GetVolunteeringAndWorkExperiencesQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
}