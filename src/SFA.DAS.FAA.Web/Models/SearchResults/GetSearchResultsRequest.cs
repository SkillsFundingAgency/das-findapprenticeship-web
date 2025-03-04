using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.SearchResults
{
    public class GetSearchResultsRequest
    {
        [FromQuery] public List<string>? RouteIds { get; set; }
        [FromQuery] public List<string>? LevelIds { get; set; }
        [FromQuery] public string? Location { get; set; }
        [FromQuery] public string? SearchTerm { get; set; }
        [FromQuery] public int? Distance { get; set; }
        [FromQuery] public int? PageNumber { get; set; }
        [FromQuery] public string? Sort {get; set; }
        [FromQuery] public bool DisabilityConfident { get; set; }
        [FromQuery] public string? RoutePath { get; set; }
        [FromQuery] public bool IncludeCompetitiveSalaryVacancies { get; set; } = false;
        [FromQuery] public bool? ExcludeNational { get; set; } = false;
    }
}
