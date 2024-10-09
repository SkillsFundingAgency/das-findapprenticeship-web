using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record GetAccountDeletionApplicationsToWithdrawApiRequest(Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"users/{CandidateId}/account-deletion";
    }
}