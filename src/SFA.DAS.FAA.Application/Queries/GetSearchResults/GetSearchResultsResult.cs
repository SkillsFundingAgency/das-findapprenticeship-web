using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsResult
{
    public long Total { get; set; }
    public long TotalCompetitiveVacanciesCount { get; set; }
    public List<VacancyAdvert> VacancyAdverts { get; set; }
    public List<RouteResponse> Routes { get; set; }
    public Location? Location { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public string Sort { get; set; }
    public string? SkipWageType { get; set; }
    public string? VacancyReference { get; set; }
    public List<LevelResponse> Levels { get; set; }
    public bool DisabilityConfident { get; set; }
    public int SavedSearchesCount { get; set; }
    public bool SearchAlreadySaved { get; set; }
    public DateTime? CandidateDateOfBirth { get; set; }
}