using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiResponse
{
    [JsonProperty("totalApprenticeshipCount")]
    public int Total { get; set; }
    [JsonProperty("location")]
    public Location? Location { get; set; }

    [JsonProperty("routes")]
    public List<Route> Routes { get; set; }
}

public class Location
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("locationName")]
    public string LocationName { get; set; }
}

public partial class Route
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }
}