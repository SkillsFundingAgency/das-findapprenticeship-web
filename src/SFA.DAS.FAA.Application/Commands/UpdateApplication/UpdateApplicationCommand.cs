using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication
{
    public record UpdateApplicationCommand : IRequest<UpdateApplicationCommandResult>
    {
        public required Guid ApplicationId { get; init; }
        public required Guid CandidateId { get; init; }
        public SectionStatus WorkHistorySectionStatus { get; init; }
    }
}