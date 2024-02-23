using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
public class GetVolunteeringOrWorkExperienceItemQuery : IRequest<GetVolunteeringOrWorkExperienceItemQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
}
