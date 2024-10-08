using SFA.DAS.FAA.Application.Queries.GetSearchResults;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class SearchResultsViewModel : ViewModelBase
{
    public bool NationalSearch { get; set; }
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public List<string> SelectedRoutes { get; set; } = new List<string>();
    public List<RouteViewModel> Routes { get; set; }
    public List<LevelViewModel> Levels { get; set; }
    public int Total { get; set; }
    public string TotalMessage => $"{(Total == 0 || NoSearchResultsByUnknownLocation ? "No" : Total.ToString("N0"))} {(Total != 1 ? "vacancies" : "vacancy")} found";
    public int? Distance { get; set; }
    public string? SearchTerm { get; set; }

    public List<VacanciesViewModel> Vacancies { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public string Sort { get; set; }
    public PaginationViewModel PaginationViewModel { get; set; } = null!;
    public string? VacancyReference { get; set; }
    public SearchApprenticeshipFilterChoices FilterChoices { get; set; } = new SearchApprenticeshipFilterChoices();
    public List<SelectedFilter> SelectedFilters { get; set; } = new();
    public bool ShowFilterOptions => SelectedFilters.Count != 0;
    public string ClearSelectedFiltersLink { get; set; } = null!;
    public bool DisabilityConfident { get; set; } = false; 
    public int SelectedLevelCount { get; set; }
    public int SelectedRouteCount { get; set; }
    public string? PageTitle { get; set; }
    public bool NoSearchResultsByUnknownLocation { get; set; }
    public List<ApprenticeshipMapData> MapData { get; set; }
    public string? MapId { get; set; }

    public bool ShowAccountCreatedBanner { get; set; } = false;

    public static implicit operator SearchResultsViewModel(GetSearchResultsResult source)
    {
        return new SearchResultsViewModel
        {
            Total = source.Total,
            Routes = source.Routes.Select(c => (RouteViewModel)c).ToList(),
            Location = source.Location?.LocationName,
            PageNumber = source.PageNumber,
            TotalPages = source.TotalPages,
            VacancyReference =source.VacancyReference,
            Sort = source.Sort,
            Levels = source.Levels.Select(l => (LevelViewModel)l).ToList()
        };
    }

    public Dictionary<string, string> RouteData { get => GetRouteData(); }

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