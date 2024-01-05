using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsResult
{
    public int Total { get; set; }
    public List<Vacancies> Vacancies { get; set; }
    public List<RouteResponse> Routes { get; set; }
    public Location? Location { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Sort { get; set; }
    public string? VacancyReference { get; set; }
    public List<LevelResponse> Levels { get; set; }
}