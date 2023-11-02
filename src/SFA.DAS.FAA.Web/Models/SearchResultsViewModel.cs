using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using static SFA.DAS.FAA.Web.Models.BrowseByInterestViewModel;

namespace SFA.DAS.FAA.Web.Models;

public class SearchResultsViewModel
{
    public bool NationalSearch { get; set; }
    public string? location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public string? WhatSearchTerm { get; set; }
    public int Total { get; set; }
    public string TotalMessage { get; private set; }

    public SearchResultsViewModel(int total)
    {
        Total = total;
        TotalMessage = GetTotalMessage(total);
    }

    private string GetTotalMessage(int total)
    {
        var totalAsText = Total.ToString("N0");
        return (Total == 0 ? "No" : totalAsText) + "apprenticeship" + (Total != 1 ? "s" : "") + "found";
    }


    public static implicit operator SearchResultsViewModel(GetSearchResultsResult source)
    {
        return new SearchResultsViewModel(source.Total);
    }
}