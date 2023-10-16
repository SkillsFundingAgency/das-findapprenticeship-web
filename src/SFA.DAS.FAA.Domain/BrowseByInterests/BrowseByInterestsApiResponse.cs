using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.BrowseByInterests;

public class BrowseByInterestsApiResponse
{
    public List<RouteResponse> Routes { get; set; }
}

public class RouteResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("id")]
    public int Id { get; set; }
}