using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record PostUserAccountDeletionApiRequest(Guid CandidateId) : IPostApiRequest
    {
        public string PostUrl => $"users/{CandidateId}/account-deletion";
        public object Data { get; set; }
    }
}
