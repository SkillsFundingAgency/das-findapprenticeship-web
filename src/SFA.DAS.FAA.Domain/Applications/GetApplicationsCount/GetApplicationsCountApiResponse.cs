using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Applications.GetApplicationsCount
{
    public record GetApplicationsCountApiResponse
    {
        [JsonProperty(nameof(Stats))]
        public List<ApplicationStats> Stats { get; init; } = [];

        public record ApplicationStats
        {
            [JsonProperty(nameof(Status))]
            public string Status { get; set; }

            [JsonProperty(nameof(Count))]
            public int Count { get; set; }
        }
    }
}