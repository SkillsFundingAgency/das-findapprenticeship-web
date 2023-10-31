namespace SFA.DAS.FAA.Web.Models;

public class SearchResultsViewModel
{
    public bool NationalSearch { get; set; }
    public string? location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }
}