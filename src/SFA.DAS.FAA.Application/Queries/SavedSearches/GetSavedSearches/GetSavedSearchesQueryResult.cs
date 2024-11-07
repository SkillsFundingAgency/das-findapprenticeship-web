using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetSavedSearches;

public class GetSavedSearchesQueryResult
{
    public List<RouteInfo> Routes { get; init; } = [];
    public List<SavedSearch> SavedSearches { get; init; } = [];
}