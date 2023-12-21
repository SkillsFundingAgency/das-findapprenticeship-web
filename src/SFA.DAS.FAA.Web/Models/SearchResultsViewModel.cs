using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class SearchResultsViewModel : ViewModelBase
{
    public bool NationalSearch { get; set; }
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public List<string> SelectedRoutes { get; set; } = new List<string>();
    public List<RouteViewModel> Routes { get; set; }
    
    public string? WhatSearchTerm { get; set; }
    public int Total { get; set; }
    public string TotalMessage  =>$"{(Total == 0 ? "No" : Total.ToString("N0"))} apprenticeship{(Total != 1 ? "s" : "")} found";
    public int? Distance { get; set; }

    public List<VacanciesViewModel> Vacancies { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public PaginationViewModel PaginationViewModel { get; set; } = null!;
    public SearchApprenticeshipFilterChoices FilterChoices { get; set; } = new SearchApprenticeshipFilterChoices();
    public List<SelectedFilter> SelectedFilters { get; set; } = [];
    public bool ShowFilterOptions => SelectedFilters.Any();
    public string ClearSelectedFiltersLink { get; set; } = null!;


    public static implicit operator SearchResultsViewModel(GetSearchResultsResult source)
    {
        return new SearchResultsViewModel
        {
            Total = source.Total,
            Routes = source.Routes.Select(c => (RouteViewModel)c).ToList(),
            Location = source.Location?.LocationName,
            PageSize = source.PageSize,
            PageNumber = source.PageNumber,
            TotalPages = source.TotalPages
        };
    }
    
    public Dictionary<string, string> RouteData { get => GetRouteData(); }
    

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