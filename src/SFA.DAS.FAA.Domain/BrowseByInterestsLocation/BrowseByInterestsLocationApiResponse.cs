using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.BrowseByInterestsLocation;

public class BrowseByInterestsLocationApiResponse
{
    [JsonProperty("location")]
    public Location Location { get; set; }
}

public partial class Location
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("locationName")]
    public string LocationName { get; set; }
}