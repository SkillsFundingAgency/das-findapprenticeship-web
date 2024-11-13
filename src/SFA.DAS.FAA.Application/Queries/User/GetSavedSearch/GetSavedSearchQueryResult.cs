using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;

public record GetSavedSearchQueryResult(SavedSearch? SavedSearch, List<RouteInfo> Routes);