using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class ConfirmUnsubscribeApiResponse
    {
        [JsonProperty("Where")]
        public string Where { get; set; }

        [JsonProperty("Distance")]
        public long Distance { get; set; }

        [JsonProperty("Categories")]
        public string[] Categories { get; set; }

        [JsonProperty("Levels")]
        public long[] Levels { get; set; }
    }
}
