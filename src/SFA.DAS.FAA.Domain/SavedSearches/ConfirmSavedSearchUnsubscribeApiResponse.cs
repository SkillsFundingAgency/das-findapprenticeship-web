using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class ConfirmSavedSearchUnsubscribeApiResponse
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("SearchTitle")]
        public string? SearchTitle { get; set; }

        [JsonProperty("What")]
        public string? What { get; set; }

        [JsonProperty("Where")]
        public string? Where { get; set; }

        [JsonProperty("Distance")]
        public long? Distance { get; set; }

        [JsonProperty("Categories")]
        public List<string>? Categories { get; set; }

        [JsonProperty("Levels")]
        public List<long>? Levels { get; set; }

        [JsonProperty("DisabilityConfident")]
        public bool DisabilityConfident { get; set; }
    }
}
