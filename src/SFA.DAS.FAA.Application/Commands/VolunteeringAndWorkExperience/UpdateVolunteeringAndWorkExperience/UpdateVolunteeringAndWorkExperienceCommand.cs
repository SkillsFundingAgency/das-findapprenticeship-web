using MediatR;

namespace SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;

public record UpdateVolunteeringAndWorkExperienceCommand : IRequest
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; init; }
    public Guid VolunteeringOrWorkExperienceId { get; init; }
    public string? CompanyName { get; init; }
    public string? Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}