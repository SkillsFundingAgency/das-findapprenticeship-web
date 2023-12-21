using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsResult
{
    public int Total { get; set; }
    public List<Vacancies> Vacancies { get; init; }
    public List<RouteResponse> Routes { get; init; }
    public Location? Location { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}