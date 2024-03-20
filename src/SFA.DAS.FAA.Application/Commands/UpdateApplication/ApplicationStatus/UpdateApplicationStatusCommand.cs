using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.ApplicationStatus;

public record UpdateApplicationStatusCommand : IRequest<UpdateApplicationStatusCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public Domain.Enums.ApplicationStatus Status { get; init; }
}