using SFA.DAS.FAA.Application.Constants;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class SearchResultsViewModel : ViewModelBase
{
    public bool NationalSearch { get; set; }
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public List<string> SelectedRoutes { get; set; } = [];
    public List<RouteViewModel> Routes { get; set; }
    public List<LevelViewModel> Levels { get; set; }
    public int Total { get; set; }
    public string TotalMessage => $"{(Total == 0 || NoSearchResultsByUnknownLocation ? "No" : Total.ToString("N0"))} {(Total != 1 ? "results" : "result")} found";
    public int? Distance { get; set; }
    public string? SearchTerm { get; set; }

    public List<VacanciesViewModel> Vacancies { get; set; } = [];
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public string Sort { get; set; }
    public PaginationViewModel PaginationViewModel { get; set; } = null!;
    public string? VacancyReference { get; set; }
    public SearchApprenticeshipFilterChoices FilterChoices { get; set; } = new();
    public List<SelectedFilter> SelectedFilters { get; set; } = [];
    public bool ShowFilterOptions => SelectedFilters.Count != 0;
    public string ClearSelectedFiltersLink { get; set; } = null!;
    public bool DisabilityConfident { get; set; } = false; 
    public int SelectedLevelCount { get; set; }
    public int SelectedRouteCount { get; set; }
    public string? PageTitle { get; set; }
    public bool NoSearchResultsByUnknownLocation { get; set; }
    public List<ApprenticeshipMapData> MapData { get; set; }
    public string? MapId { get; set; }
    public long TotalCompetitiveVacanciesCount { get; set; }
    public string? SkipWageType { get; set; }

    public string? CompetitiveSalaryRoutePath { get; set; }
    public string? CompetitiveSalaryBannerText =>
        string.IsNullOrEmpty(SkipWageType)
            ? "Hide vacancies without a listed annual wage"
            : "Show vacancies without a listed annual wage";
    
    public bool ShowAccountCreatedBanner { get; set; } = false;
    public string? EncodedRequestData { get; set; }
    public bool SearchAlreadySaved { get; set; }
    public bool SavedSearchLimitReached  { get; set; }

    public bool ShowCompetitiveSalaryBanner =>
        string.Equals(Sort, VacancySort.SalaryAsc.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
        string.Equals(Sort, VacancySort.SalaryDesc.ToString(), StringComparison.CurrentCultureIgnoreCase);

    public static implicit operator SearchResultsViewModel(GetSearchResultsResult source)
    {
        return new SearchResultsViewModel
        {
            Total = string.IsNullOrEmpty(source.SkipWageType)
                ? source.Total
                : source.Total + source.TotalCompetitiveVacanciesCount,
            TotalCompetitiveVacanciesCount = source.TotalCompetitiveVacanciesCount,
            Routes = source.Routes.Select(c => (RouteViewModel)c).ToList(),
            Location = source.Location?.LocationName,
            PageNumber = source.PageNumber,
            TotalPages = source.TotalPages,
            VacancyReference =source.VacancyReference,
            Sort = source.Sort,
            Levels = source.Levels.Select(l => (LevelViewModel)l).ToList(),
            SavedSearchLimitReached = source.SavedSearchesCount >= Constants.SavedSearchLimit,
            SearchAlreadySaved = source.SearchAlreadySaved,
            SkipWageType = source.SkipWageType,
        };
    }

    public Dictionary<string, string> RouteData => GetRouteData();

    public string? PageBackLinkRoutePath { get; set; }

    private Dictionary<string, string> GetRouteData()
    {
        var result = new Dictionary<string, string>();

        if (SelectedRouteIds is null)
        {
            return result;
        }
        for (var i = 0; i < SelectedRouteIds.Count; i++)
        {
            var selectedRouteId = SelectedRouteIds[i];
            result.Add($"routeIds[{i}]", selectedRouteId.ToString());
        }

        return result;
    }
}