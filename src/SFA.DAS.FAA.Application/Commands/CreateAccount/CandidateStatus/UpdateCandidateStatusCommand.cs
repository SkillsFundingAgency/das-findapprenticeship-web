using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus
{
    public record UpdateCandidateStatusCommand : IRequest<UpdateCandidateStatusCommandResult>
    {
        public required string GovIdentifier { get; set; }
        public required string CandidateEmail { get; set; }
    }
}