using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;

public class GetConfirmUnsubscribeResult
{
    public List<RouteInfo> Routes { get; init; } = [];
    public SavedSearch? SavedSearch { get; init; }
}