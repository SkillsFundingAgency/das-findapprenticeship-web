using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.DisabilityConfident;

public record UpdateDisabilityConfidenceApplicationCommand : IRequest<UpdateDisabilityConfidenceApplicationCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public SectionStatus DisabilityConfidenceSectionStatus { get; init; }
}