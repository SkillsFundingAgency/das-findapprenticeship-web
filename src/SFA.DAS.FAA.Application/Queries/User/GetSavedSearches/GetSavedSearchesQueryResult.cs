using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;

public record GetSavedSearchesQueryResult(List<SavedSearch> SavedSearches, List<RouteInfo> Routes);