using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.User;

public record SavedSearchViewModel(
    string Title,
    Guid Id,
    string? SearchTerm,
    decimal? Distance,
    List<string>? SelectedRoutes,
    List<int>? SelectedLevelIds,
    bool DisabilityConfident,
    string? Location)
{
    public static SavedSearchViewModel From(SavedSearch source, List<RouteInfo> routes)
    {
        var definingCharacteristic = source.SearchParameters switch
        {
            { SearchTerm: not null } => source.SearchParameters.SearchTerm,
            { SelectedRouteIds: { Count: 1 } } => routes.First(route => route.Id == source.SearchParameters.SelectedRouteIds.First()).Name,
            { SelectedRouteIds: { Count: > 1 } } => $"{source.SearchParameters.SelectedRouteIds.Count} categories",
            { SelectedLevelIds.Count: 1 } => $"Level {source.SearchParameters.SelectedLevelIds.First()}",
            { SelectedLevelIds.Count: > 1 } => $"{source.SearchParameters.SelectedLevelIds.Count} apprenticeship levels",
            { DisabilityConfident: true } => $"Disability Confident",
            _ => "All apprenticeships"
        };

        var location = source.SearchParameters.Location is null
            ? "all of England"
            : $"{source.SearchParameters.Location}";
                
        var title = $"{definingCharacteristic} in {location}";
            
        return new SavedSearchViewModel(
            title,
            source.Id,
            source.SearchParameters.SearchTerm,
            source.SearchParameters.Distance,
            source.SearchParameters.SelectedRouteIds?.Select(category => routes.FirstOrDefault(route => route.Id == category)?.Name ?? string.Empty).ToList(),
            source.SearchParameters.SelectedLevelIds,
            source.SearchParameters.DisabilityConfident,
            source.SearchParameters.Location
        );
    }
}

public record SavedSearchesViewModel(List<SavedSearchViewModel> SavedSearches);