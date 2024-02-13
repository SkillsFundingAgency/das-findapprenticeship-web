using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
public record UpdateVolunteeringAndWorkExperienceApplicationCommand : IRequest<UpdateVolunteeringAndWorkExperienceApplicationCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public SectionStatus VolunteeringAndWorkExperienceSectionStatus { get; init; }
}
