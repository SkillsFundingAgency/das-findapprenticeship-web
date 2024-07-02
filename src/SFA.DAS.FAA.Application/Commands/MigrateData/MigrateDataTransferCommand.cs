using MediatR;

namespace SFA.DAS.FAA.Application.Commands.MigrateData
{
    public class MigrateDataTransferCommand : IRequest<Unit>
    {
        public required Guid CandidateId { get; init; }
        public required string EmailAddress { get; init; }
    }
}
