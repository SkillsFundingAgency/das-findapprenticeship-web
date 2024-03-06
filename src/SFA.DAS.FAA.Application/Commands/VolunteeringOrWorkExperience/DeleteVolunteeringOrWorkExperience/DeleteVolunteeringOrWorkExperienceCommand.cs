using MediatR;

namespace SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class DeleteVolunteeringOrWorkExperienceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
