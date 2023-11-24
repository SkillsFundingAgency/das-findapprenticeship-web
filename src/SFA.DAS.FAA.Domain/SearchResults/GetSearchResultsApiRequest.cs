using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiRequest : IGetApiRequest
{
    private readonly string? _location;
    private string _routes;
    private readonly int? _distance;
    private readonly string? _searchTerm;

    public GetSearchResultsApiRequest(string? location, List<string>? routes, int? distance, string? searchTerm)
    {
        _location = location;
        _routes = routes != null ? string.Join("&routes=", routes) : "";
        _distance = distance;
        _searchTerm = searchTerm;
    }

    public string GetUrl => $"searchapprenticeships/searchResults?location={_location}&routes={_routes}&distance={_distance}&searchTerm={_searchTerm}";
}