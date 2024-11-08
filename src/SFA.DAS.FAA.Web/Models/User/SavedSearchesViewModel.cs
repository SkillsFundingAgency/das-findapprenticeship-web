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
    string? Location,
    bool ReadOnly = false)
{
    public static string CreateTitle(SearchParameters searchParameters, List<RouteInfo> routes)
    {
        var definingCharacteristic = searchParameters switch
        {
            { SearchTerm: not null } => searchParameters.SearchTerm,
            { SelectedRouteIds: { Count: 1 } } => routes.First(route => route.Id == searchParameters.SelectedRouteIds.First()).Name,
            { SelectedRouteIds: { Count: > 1 } } => $"{searchParameters.SelectedRouteIds.Count} categories",
            { SelectedLevelIds.Count: 1 } => $"Level {searchParameters.SelectedLevelIds.First()}",
            { SelectedLevelIds.Count: > 1 } => $"{searchParameters.SelectedLevelIds.Count} apprenticeship levels",
            { DisabilityConfident: true } => $"Disability Confident",
            _ => "All apprenticeships"
        };

        var location = searchParameters.Location is null
            ? "all of England"
            : $"{searchParameters.Location}";
                
        return $"{definingCharacteristic} in {location}";
    }
    
    public static SavedSearchViewModel From(SavedSearch source, List<RouteInfo> routes, bool readOnly = false)
    {
        return new SavedSearchViewModel(
            CreateTitle(source.SearchParameters, routes),
            source.Id,
            source.SearchParameters.SearchTerm,
            source.SearchParameters.Distance,
            source.SearchParameters.SelectedRouteIds?.Select(category => routes.FirstOrDefault(route => route.Id == category)?.Name ?? string.Empty).ToList(),
            source.SearchParameters.SelectedLevelIds,
            source.SearchParameters.DisabilityConfident,
            source.SearchParameters.Location,
            readOnly
        );
    }
}

public record SavedSearchesViewModel(List<SavedSearchViewModel> SavedSearches, bool ShowDeletedBanner = false, string? DeletedSavedSearchTitle = null);