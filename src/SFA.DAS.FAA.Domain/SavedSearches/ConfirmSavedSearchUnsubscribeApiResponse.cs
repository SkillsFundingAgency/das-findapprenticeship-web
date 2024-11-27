using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SavedSearches.Dto;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class ConfirmSavedSearchUnsubscribeApiResponse
    {
        public List<RouteResponse> Routes { get; init; }
        public SavedSearchDto? SavedSearch { get; set; }
    }
}
