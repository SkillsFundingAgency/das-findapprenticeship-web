using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.BrowseByInterests;

public class Route
{
    [JsonProperty("route")]
    public string Routes { get; set; }
}