using MediatR;

namespace SFA.DAS.FAA.Application.Commands.User.PostAccountDeletion
{
    public record AccountDeletionCommand(Guid CandidateId) : IRequest;
}