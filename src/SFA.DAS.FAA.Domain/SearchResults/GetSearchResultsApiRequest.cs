using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiRequest : IGetApiRequest
{
    private readonly string? _location;
    private readonly string _routes;
    private readonly string _levels;
    private readonly int? _distance;
    private readonly string? _searchTerm;
    private readonly int? _pageNumber;
    private readonly int? _pageSize;
    private readonly VacancySort _sort;
    private readonly bool _disabilityConfident;

    public GetSearchResultsApiRequest(string? location, IReadOnlyCollection<string>? routes, IReadOnlyCollection<string>? levels, int? distance, string? searchTerm, int? pageNumber, int? pageSize, VacancySort sort, bool disabilityConfident)
    {
        _location = location;
        _routes = routes != null ? string.Join("&routeIds=", routes) : string.Empty;
        _levels = levels != null ? string.Join("&levelIds=", levels) : string.Empty;
        _distance = distance;
        _searchTerm = searchTerm;
        _pageNumber = pageNumber;
        _pageSize = pageSize;
        _sort = sort;
        _disabilityConfident = disabilityConfident;
    }

    public string GetUrl => $"searchapprenticeships/searchResults?location={_location}&routeIds={_routes}&distance={_distance}&searchTerm={_searchTerm}&pageNumber={_pageNumber}&pageSize={_pageSize}&sort={_sort}&levelIds={_levels}&isDisabilityConfident{_disabilityConfident}";
}