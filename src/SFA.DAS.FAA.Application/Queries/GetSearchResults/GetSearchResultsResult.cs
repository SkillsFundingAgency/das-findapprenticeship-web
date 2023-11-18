using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsResult
{
    public int Total { get; set; }
    public List<Route> Routes { get; set; }
    public Location? Location { get; set; }
}