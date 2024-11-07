using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class SearchApprenticeshipsApiResponse
{
    [JsonProperty("totalApprenticeshipCount")]
    public int Total { get; set; }
    [JsonProperty("locationSearched")]
    public bool LocationSearched { get; set; }
    [JsonProperty("location")]
    public Location? Location { get; set; }
    [JsonProperty("savedSearches")]
    public List<SavedSearchDto> SavedSearches { get; set; }
    [JsonProperty("routes")]
    public List<RouteResponse> Routes { get; set; }
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