using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Candidates
{
    public record UpdateCandidateStatusApiRequest(string GovIdentifier, UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"candidates/{GovIdentifier}/status";
        public object Data { get; set; } = Body;

        public class UpdateCandidateStatusApiRequestData
        {
            public required string Email { get; set; }
            public required UserStatus Status { get; set; }
        }
    }
}