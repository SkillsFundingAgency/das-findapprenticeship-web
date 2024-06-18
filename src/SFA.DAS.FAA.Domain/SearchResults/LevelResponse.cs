using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults
{
    public class LevelResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int Code { get; set; }
    }
}
