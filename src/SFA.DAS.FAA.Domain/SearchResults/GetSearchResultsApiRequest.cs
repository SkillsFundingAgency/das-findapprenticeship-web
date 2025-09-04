using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class GetSearchResultsApiRequest : IGetApiRequest
{
    private readonly string? _location;
    private readonly string? _routes;
    private readonly string? _levels;
    private readonly int? _distance;
    private readonly string? _searchTerm;
    private readonly int? _pageNumber;
    private readonly int? _pageSize;
    private readonly VacancySort _sort;
    private readonly WageType? _skipWageType;
    private readonly bool _disabilityConfident;
    private readonly string? _candidateId;
    private readonly bool? _excludeNational;
    private readonly List<ApprenticeshipTypes>? _apprenticeshipTypes;
    
    public GetSearchResultsApiRequest(string? location,
        IReadOnlyCollection<string>? routes,
        IReadOnlyCollection<string>? levels,
        int? distance,
        string? searchTerm,
        int? pageNumber,
        int? pageSize,
        VacancySort sort,
        WageType? skipWageType,
        bool disabilityConfident,
        string? candidateId,
        bool? excludeNational,
        List<ApprenticeshipTypes>? apprenticeshipTypes)
    {
        _location = location;
        _routes = routes != null ? string.Join("&routeIds=", routes) : null;
        _levels = levels != null ? string.Join("&levelIds=", levels) : null;
        _distance = distance;
        _searchTerm = searchTerm;
        _pageNumber = pageNumber;
        _pageSize = pageSize;
        _sort = sort;
        _skipWageType = skipWageType;
        _disabilityConfident = disabilityConfident;
        _candidateId = candidateId;
        _excludeNational = excludeNational;
        _apprenticeshipTypes = apprenticeshipTypes;
    }

    public string GetUrl {
        get
        {
            var url =
                $"searchapprenticeships/searchResults?location={HttpUtility.UrlEncode(_location)}" +
                $"&distance={_distance}" +
                $"&searchTerm={HttpUtility.UrlEncode(_searchTerm)}" +
                $"&pageNumber={_pageNumber}" +
                $"&pageSize={_pageSize}" +
                $"&sort={_sort}" +
                $"&disabilityConfident={_disabilityConfident}" +
                $"&candidateId={_candidateId}" +
                $"&skipWageType={_skipWageType}" +
                (_routes is not null ? $"&routeIds={_routes}" : null) +
                (_levels is not null ? $"&levelIds={_levels}" : null) +
                $"&excludeNational={_excludeNational}";

            url = QueryHelpers.AddQueryString(url, [new KeyValuePair<string, StringValues>("apprenticeshipTypes", new StringValues(_apprenticeshipTypes?.Select(x => $"{x}").ToArray()))]);
            return url;
        }
    }
}