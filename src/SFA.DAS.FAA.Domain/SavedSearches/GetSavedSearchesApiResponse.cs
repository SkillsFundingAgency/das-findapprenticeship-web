using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.SavedSearches.Dto;

namespace SFA.DAS.FAA.Domain.SavedSearches;

public class GetSavedSearchesApiResponse
{
    public List<SavedSearchDto> SavedSearches { get; init; }
    public List<RouteResponse> Routes { get; init; }
}