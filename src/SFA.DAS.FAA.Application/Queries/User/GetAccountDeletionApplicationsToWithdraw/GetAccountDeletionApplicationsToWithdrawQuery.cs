using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw
{
    public record GetAccountDeletionApplicationsToWithdrawQuery(Guid CandidateId) : IRequest<GetAccountDeletionApplicationsToWithdrawQueryResult>;
}
