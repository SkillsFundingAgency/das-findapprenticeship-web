using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiRequest : IGetApiRequest
{
    private readonly string? _location;
    private readonly List<string>? _routes;
    private readonly int? _distance;
    public GetSearchResultsApiRequest(string? location, List<string>? routes, int? distance)
    {
        _location = location;
        _routes = routes;
        _distance = distance;
    }

    public string GetUrl => $"vacancies?location={_location}&routes={_routes}&distance={_distance}";
}