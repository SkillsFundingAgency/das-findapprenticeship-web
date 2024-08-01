using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Candidates
{
    public class UpdateCandidateStatusApiRequest(string govIdentifier, UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"candidates/{govIdentifier}/status";
        public object Data { get; set; } = body;

        public class UpdateCandidateStatusApiRequestData
        {
            public required string Email { get; set; }
            public required UserStatus Status { get; set; }
        }
    }
}