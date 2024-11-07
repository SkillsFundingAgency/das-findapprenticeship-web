using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex
{
    public class SearchApprenticeshipsApiResponse
    {
        [JsonProperty("totalApprenticeshipCount")]
        public int Total { get; set; }
        [JsonProperty("locationSearched")]
        public bool LocationSearched { get; set; }
        [JsonProperty("location")]
        public Location? Location { get; set; }
        [JsonProperty("savedSearches")]
        public List<SavedSearchItem> SavedSearches { get; set; }
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

    public class SavedSearchItem
    {
        [JsonProperty("what")]
        public string What { get; set; }
        [JsonProperty("where")]
        public string Where { get; set; }
        [JsonProperty("categories")]
        public List<string> Categories  { get; set; }
        [JsonProperty("levels")]
        public List<int> Levels  { get; set; }
        [JsonProperty("disabilityConfident")]
        public bool DisabilityConfident  { get; set; }
    }
}

