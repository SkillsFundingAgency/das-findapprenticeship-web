using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.SearchResults
{
    public class GetSearchResultsRequest
    {
        [FromQuery] public List<string>? RouteIds { get; set; }
        [FromQuery] public string? Location { get; set; }
        [FromQuery] public string? SearchTerm { get; set; }
        [FromQuery] public int? Distance { get; set; }
        [FromQuery] public int? PageNumber { get; set; }
    }
}
