using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory
{
    public record UpdateWorkHistoryApplicationCommand : IRequest<UpdateWorkHistoryApplicationCommandResult>
    {
        public required Guid ApplicationId { get; init; }
        public required Guid CandidateId { get; init; }
        public SectionStatus WorkHistorySectionStatus { get; init; }
    }
}