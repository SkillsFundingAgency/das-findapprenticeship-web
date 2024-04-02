using MediatR;

namespace SFA.DAS.FAA.Application.Commands.DisabilityConfident;

public record UpdateDisabilityConfidenceApplicationCommand : IRequest<UpdateDisabilityConfidenceApplicationCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public bool IsSectionCompleted { get; init; }
}