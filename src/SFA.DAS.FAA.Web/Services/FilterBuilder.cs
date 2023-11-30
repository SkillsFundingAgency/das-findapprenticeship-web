using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.Services
{
    public static class FilterBuilder
    {
        public static string BuildFullQueryString(GetSearchResultsRequest request, IUrlHelper url)
        {
            var fullQueryParameters = BuildQueryParameters(request);
            return BuildQueryString(url, fullQueryParameters, "none")!;
        }

        private static IEnumerable<string> BuildQueryParameters(GetSearchResultsRequest request)
        {
            var queryParameters = new List<string>();

            if (request.RouteIds != null)
                queryParameters.AddRange(request.RouteIds.Select(isActive => "routeIds=" + isActive));

            return queryParameters;
        }

        private static string? BuildQueryString(IUrlHelper url, IEnumerable<string> queryParameters, string filterToRemove)
        {
            var queryParametersToBuild = queryParameters.Where(s => s != filterToRemove).ToList();

            return queryParametersToBuild.Any() 
                ? $"{url.RouteUrl(RouteNames.SearchResults)}?{string.Join('&', queryParametersToBuild)}" 
                : url.RouteUrl(RouteNames.SearchResults);
        }
    }
}
