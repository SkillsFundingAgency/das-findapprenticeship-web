using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.MigrateApplications
{
    public record PostMigrateLegacyApplicationsApiRequest(Guid CandidateId, PostMigrateLegacyApplicationsApiRequest.PostMigrateLegacyApplicationsRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{CandidateId}/migrate";
        public object Data { get; set; } = Body;

        public class PostMigrateLegacyApplicationsRequestData
        {
            public required string EmailAddress { get; set; }
        }
    }
}