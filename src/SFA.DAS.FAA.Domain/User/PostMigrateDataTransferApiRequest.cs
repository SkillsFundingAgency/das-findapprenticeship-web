using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record PostMigrateDataTransferApiRequest(Guid CandidateId, PostMigrateDataTransferApiRequest.PostMigrateDataTransferApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"users/{CandidateId}/migrate";
        public object Data { get; set; } = Body;

        public class PostMigrateDataTransferApiRequestData
        {
            public required string EmailAddress { get; set; }
        }
    }
}