using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteVolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceQuery : IRequest<GetDeleteVolunteeringOrWorkExperienceQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
}
