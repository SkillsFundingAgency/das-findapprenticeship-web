using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;
using System.Web;

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
            
            filters.AddSingleFilterItem(urlHelper, fullQueryParameters, "What",
                request.SearchTerm,[$"searchTerm={request.SearchTerm}"]);
            filters.AddSingleFilterItem(urlHelper, fullQueryParameters, "Where",
                string.IsNullOrEmpty(request.Location) ? "" : $"{request.Location} ({(request.Distance != null ? $"within {request.Distance} miles" : "across England")})",
                [$"location={request.Location}", $"distance={(request.Distance == null ? "all" : request.Distance)}", $"sort={VacancySort.DistanceAsc}"]);
            
            filters.AddFilterItems(urlHelper, fullQueryParameters, request.RouteIds, "Job category", "routeIds", filterChoices.JobCategoryChecklistDetails.Lookups.ToList());
            filters.AddFilterItems(urlHelper, fullQueryParameters, request.LevelIds, "Apprenticeship level", "levelIds", filterChoices.CourseLevelsChecklistDetails.Lookups.ToList());
            
            if(request.DisabilityConfident)
            {
                filters.AddSingleFilterItem(urlHelper, fullQueryParameters, "Disability Confident", "Only show Disability Confident companies", [$"DisabilityConfident={request.DisabilityConfident}"]);
            }

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
            
            if (!string.IsNullOrEmpty(request.Sort))
                queryParameters.Add($"sort={request.Sort}");
            if (request.LevelIds != null)
                queryParameters.AddRange(request.LevelIds.Select(isActive => "levelIds=" + isActive));

            if(request.DisabilityConfident)
            {
                queryParameters.Add($"DisabilityConfident={request.DisabilityConfident}");
            }

            if (request.IncludeCompetitiveSalaryVacancies)
                queryParameters.Add($"IncludeCompetitiveSalaryVacancies=true");
            else
                queryParameters.Remove("IncludeCompetitiveSalaryVacancies=false");

            return queryParameters;
        }

        public static string ReplaceQueryStringParam(string currentPageUrl, string paramToReplace, string newValue)
        {
            var urlWithoutQuery = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl[..currentPageUrl.IndexOf('?')]
                : currentPageUrl;

            var queryString = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl[currentPageUrl.IndexOf('?')..]
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);

            if (queryParamList[paramToReplace] != null)
            {
                queryParamList[paramToReplace] = newValue;
            }
            else
            {
                queryParamList.Add(paramToReplace, newValue);
            }
            return $"{urlWithoutQuery}?{queryParamList}";
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
        
        private static void AddSingleFilterItem(
            this ICollection<SelectedFilter> filters, 
            IUrlHelper urlHelper,
            List<string> fullQueryParameters,
            string fieldName,
            string value,
            List<string> filterToRemove)
        {
            if (!string.IsNullOrEmpty(value))
            {
                filters.Add(new SelectedFilter
                {
                    FieldName = fieldName,
                    FieldOrder = -1,
                    Filters =
                    [
                        new()
                        {
                            ClearFilterLink = BuildQueryString(urlHelper, fullQueryParameters, filterToRemove),
                            Order = 0,
                            Value = value
                        }
                    ]
                });
            }
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
