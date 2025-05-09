using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Applications.GetApplicationsCount
{
    public record GetApplicationsCountApiResponse
    {
        [JsonProperty(nameof(ApplicationIds))]
        public List<Guid> ApplicationIds { get; set; } = [];
        [JsonProperty(nameof(Status))]
        public string Status { get; set; }
        [JsonProperty(nameof(Count))]
        public int Count { get; set; }
    }
}