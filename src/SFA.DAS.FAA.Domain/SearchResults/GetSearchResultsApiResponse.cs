using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.BrowseByInterests;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiResponse
{
    [JsonProperty("totalApprenticeshipCount")]
    public int Total { get; set; }

    [JsonProperty("totalFound")]
    public int TotalFound { get; set; }
    
    [JsonProperty("vacancies")]
    public List<Vacancies> Vacancies { get; set; }

    [JsonProperty("location")]
    public Location? Location { get; set; }

    [JsonProperty("routes")]
    public List<RouteResponse> Routes { get; set; }

    [JsonProperty("PageNumber")]
    public int PageNumber { get; set; }

    [JsonProperty("PageSize")]
    public int PageSize { get; set; }

    [JsonProperty("TotalPages")]
    public int TotalPages { get; set; }

    [JsonProperty("VacancyReference")]
    public string VacancyReference { get; set; }

    [JsonProperty("Categories")]
    public List<string>? Categories { get; set; }
    [JsonProperty("levels")]
    public List<LevelResponse> Levels { get; set; }
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