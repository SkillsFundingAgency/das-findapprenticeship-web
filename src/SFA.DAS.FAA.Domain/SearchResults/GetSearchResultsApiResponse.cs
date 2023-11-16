using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiResponse
{
    [JsonProperty("totalFiltered")]
    public int Total { get; set; }
    [JsonProperty("apprenticeshipVacancies")]
    public List <Vacancies> Vacancies { get; set; }
 
}