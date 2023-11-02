using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiResponse
{
    [JsonProperty("totalApprenticeshipCount")]
    public int Total { get; set; }
}