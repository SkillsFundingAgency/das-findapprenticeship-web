using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications
{
    public class MigrateLegacyApplicationsCommand : IRequest<Unit>
    {
        public required Guid CandidateId { get; init; }
        public required string EmailAddress { get; init; }
    }
}
