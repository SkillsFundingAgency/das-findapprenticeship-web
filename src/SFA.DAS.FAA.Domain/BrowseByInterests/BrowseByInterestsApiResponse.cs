using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.BrowseByInterests;

public class BrowseByInterestsApiResponse
{
    public List<RouteResponse> Routes { get; set; }
}

public class RouteResponse
{
    [JsonProperty("route")]
    public string Route { get; set; }
    [JsonProperty]
    public int Id { get; set; }
}