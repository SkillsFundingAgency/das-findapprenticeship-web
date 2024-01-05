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
            return BuildQueryString(url, fullQueryParameters, ["none"])!;
        }

        public static List<SelectedFilter> Build(
            GetSearchResultsRequest request,
            IUrlHelper urlHelper,
            SearchApprenticeshipFilterChoices filterChoices)
        {
            var filters = new List<SelectedFilter>();
            var fullQueryParameters = BuildQueryParameters(request);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filters.Add(new SelectedFilter
                {
                    FieldName = "What",
                    FieldOrder = -1,
                    Filters =
                    [
                        new()
                        {
                            ClearFilterLink = BuildQueryString(urlHelper, fullQueryParameters, [$"searchTerm={request.SearchTerm}"]),
                            Order = 0,
                            Value = $"{request.SearchTerm}"
                        }
                    ]
                });
            }
            if (!string.IsNullOrEmpty(request.Location))
            {
                var distanceValue = request.Distance != null ? $"within {request.Distance} miles" : "Across England";
                filters.Add(new SelectedFilter
                {
                    FieldName = "Where",
                    FieldOrder = 0,
                    Filters =
                    [
                        new()
                        {
                            ClearFilterLink = BuildQueryString(urlHelper, fullQueryParameters, [$"location={request.Location}",$"distance={(request.Distance == null ? "all" : request.Distance)}"]),
                            Order = 0,
                            Value = $"{request.Location} ({distanceValue})"
                        }
                    ]
                });
            }
            
            filters.AddFilterItems(urlHelper, fullQueryParameters, request.RouteIds, "Job Category", "routeIds", filterChoices.JobCategoryChecklistDetails.Lookups.ToList());
            
            return filters;
        }

        private static List<string> BuildQueryParameters(GetSearchResultsRequest request)
        {
            var queryParameters = new List<string>();
            if (!string.IsNullOrEmpty(request.SearchTerm))
                queryParameters.Add($"searchTerm={request.SearchTerm}");
            
            if (!string.IsNullOrEmpty(request.Location))
            {
                queryParameters.Add($"location={request.Location}");
                if (request.Distance is not null)
                    queryParameters.Add($"distance={request.Distance}");
                else
                    queryParameters.Add("distance=all");
            }
                
            if (request.RouteIds != null)
                queryParameters.AddRange(request.RouteIds.Select(isActive => "routeIds=" + isActive));
            return queryParameters;
        }

        private static string? BuildQueryString(IUrlHelper url, IEnumerable<string> queryParameters, List<string> filterToRemove)
        {
            var queryParametersToBuild = queryParameters.Where(s => !filterToRemove.Contains(s)).ToList();
            return queryParametersToBuild.Count > 0 
                ? $"{url.RouteUrl(RouteNames.SearchResults)}?{string.Join('&', queryParametersToBuild)}" 
                : url.RouteUrl(RouteNames.SearchResults);
        }
        private static void AddFilterItems(
            this ICollection<SelectedFilter> filters,
            IUrlHelper url,
            IReadOnlyCollection<string> fullQueryParameters,
            List<string>? selectedValues,
            string fieldName,
            string parameterName,
            IReadOnlyCollection<ChecklistLookup> lookups)
        {
            if (selectedValues is not {Count: > 0}) return;

            var filter = new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filters.Count + 1
            };

            var index = 0;
            foreach (var selectedValue in selectedValues.Select(selectedValue => lookups.FirstOrDefault(l => l.Value == selectedValue)))
            {
                if (selectedValue != null)
                    filter.Filters.Add(BuildFilterItem(url, fullQueryParameters,
                        $"{parameterName}={selectedValue.Value}", selectedValue.Name, ++index));
            }

            filters.Add(filter);
        }

        private static SearchApprenticeFilterItem BuildFilterItem(
            IUrlHelper url,
            IEnumerable<string> queryParameters,
            string filterToRemove,
            string filterValue,
            int filterFieldOrder)
            =>
                new()
                {
                    ClearFilterLink = BuildQueryString(url, queryParameters, [filterToRemove]),
                    Order = filterFieldOrder,
                    Value = filterValue
                };
    }
}
