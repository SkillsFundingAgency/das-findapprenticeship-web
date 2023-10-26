using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.LocationsBySearch;
public class GetLocationsBySearchApiResponse
{
    [JsonProperty("locations")]
    public List<LocationItem>? LocationItems { get; set; }

    public class LocationItem
    {
        [JsonProperty("location")]
        public LocationPoint? LocationPoint { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class LocationPoint
    {
        [JsonProperty("geoPoint")]
        public List<double>? GeoPoint { get; set; }
    }
}
