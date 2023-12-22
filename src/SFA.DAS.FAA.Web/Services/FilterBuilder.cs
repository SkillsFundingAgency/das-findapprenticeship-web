using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
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

        public static List<SelectedFilter> Build(
            GetSearchResultsRequest request,
            IUrlHelper urlHelper,
            IEnumerable<ChecklistLookup> categoriesLookups)
        {
            var filters = new List<SelectedFilter>();
            var fullQueryParameters = BuildQueryParameters(request);
            filters.AddFilterItems(urlHelper, fullQueryParameters, request.RouteIds, "Job Category", "routeIds", categoriesLookups.ToList());
            return filters;
        }

        private static List<string> BuildQueryParameters(GetSearchResultsRequest request)
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
        private static void AddFilterItems(
            this List<SelectedFilter> filters,
            IUrlHelper url,
            List<string> fullQueryParameters,
            List<string>? selectedValues,
            string fieldName,
            string parameterName,
            List<ChecklistLookup> lookups)
        {
            if (selectedValues == null || !selectedValues.Any()) return;

            var filter = new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filters.Count + 1
            };

            var index = 0;
            foreach (var selectedValue in selectedValues.Select(selectedValue => lookups.First(l => l.Value == selectedValue)))
            {
                filter.Filters.Add(BuildFilterItem(url, fullQueryParameters, $"{parameterName}={selectedValue.Value}", selectedValue.Name, ++index));
            }

            filters.Add(filter);
        }

        private static SearchApprenticeFilterItem BuildFilterItem(
            IUrlHelper url,
            List<string> queryParameters,
            string filterToRemove,
            string filterValue,
            int filterFieldOrder)
            =>
                new()
                {
                    ClearFilterLink = BuildQueryString(url, queryParameters, filterToRemove),
                    Order = filterFieldOrder,
                    Value = filterValue
                };
    }
}
