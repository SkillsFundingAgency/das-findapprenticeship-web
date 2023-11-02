namespace SFA.DAS.FAA.Web.Models;

public class SearchResultsViewModel
{
    public bool NationalSearch { get; set; }
    public string? location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
    public string? WhatSearchTerm { get; set; }
    public int Total { get; set; }
    public string TotalMessage => GetTotalMessage();


    private string GetTotalMessage()
    {
        var totalAsText = Total.ToString("N0");
        return (Total == 0 ? "No" : totalAsText + "apprenticeship" + (Total != 1 ? "s" : "") + "found";
    }
}