using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using System.Reflection.Emit;

namespace SFA.DAS.FAA.Web.Models;

public class SearchResultsViewModel : ViewModelBase
{
    public bool NationalSearch { get; set; }
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    
    public string? WhatSearchTerm { get; set; }
    public int Total { get; set; }
    public string TotalMessage { get; private set; }
    public int? Distance { get; set; }

    public List<VacanciesViewModel> vacancies { get; set; }
    
    public SearchResultsViewModel(int total)
    {
        Total = total;
        TotalMessage = GetTotalMessage(total);
    }

    private string GetTotalMessage(int total)
    {
        var totalAsText = Total.ToString("N0");
        return (Total == 0 ? "No" : totalAsText) + " apprenticeship" + (Total != 1 ? "s" : "") + " found";
    }


    public static implicit operator SearchResultsViewModel(GetSearchResultsResult source)
    {
        return new SearchResultsViewModel(source.Total);
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