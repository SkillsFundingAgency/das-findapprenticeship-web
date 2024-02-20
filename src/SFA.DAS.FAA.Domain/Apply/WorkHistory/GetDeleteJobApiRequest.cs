using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public class GetDeleteJobApiRequest(Guid ApplicationId, Guid CandidateId, Guid JobId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/jobs/{JobId}/delete?candidateId={CandidateId}";
    }
    public class GetDeleteJobApiResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("employer")]
        public string Employer { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
