﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.Models.User;

public record SavedSearchViewModel(
    string Title,
    Guid Id,
    string? SearchTerm,
    decimal? Distance,
    List<string>? SelectedRoutes,
    List<int>? SelectedLevelIds,
    bool DisabilityConfident,
    bool? ExcludeNational,
    string? Location,
    string? SearchUrl,
    bool ReadOnly = false)
{
    public static SavedSearchViewModel From(SavedSearch source, List<RouteInfo> routes, IUrlHelper? urlHelper = null, bool readOnly = false)
    {
        var definingCharacteristic = source.SearchParameters switch
        {
            { SearchTerm: not null } => source.SearchParameters.SearchTerm,
            { SelectedRouteIds: { Count: 1 } } => routes.First(route => route.Id == source.SearchParameters.SelectedRouteIds.First()).Name,
            { SelectedRouteIds: { Count: > 1 } } => $"{source.SearchParameters.SelectedRouteIds.Count} categories",
            { SelectedLevelIds.Count: 1 } => $"Level {source.SearchParameters.SelectedLevelIds.First()}",
            { SelectedLevelIds.Count: > 1 } => $"{source.SearchParameters.SelectedLevelIds.Count} apprenticeship levels",
            { DisabilityConfident: true } => "Disability Confident",
            { ExcludeNational: true } => "Exclude National",
            _ => "All apprenticeships"
        };

        var locationForTitle = "all of England";
        var location = "All of England";
        if (source.SearchParameters.Location?.Trim() is not null and not "")
        {
            locationForTitle = source.SearchParameters.Location;
            location = source.SearchParameters.Distance is not null ? $"{source.SearchParameters.Location} (within {source.SearchParameters.Distance} miles)" : $"{source.SearchParameters.Location} (across England)";
        }

        var title = $"{definingCharacteristic} in {locationForTitle}";
        var url = urlHelper == null
            ? string.Empty
            : FilterBuilder.BuildFullQueryString(new GetSearchResultsRequest
            {
                Location = source.SearchParameters.Location,
                SearchTerm = source.SearchParameters.SearchTerm,
                Distance = source.SearchParameters.Distance,
                DisabilityConfident = source.SearchParameters.DisabilityConfident,
                ExcludeNational = source.SearchParameters.ExcludeNational,
                LevelIds = source.SearchParameters.SelectedLevelIds != null ? source.SearchParameters.SelectedLevelIds.Select(x=>x.ToString()).ToList() : [],
                RouteIds = source.SearchParameters.SelectedRouteIds != null ? source.SearchParameters.SelectedRouteIds.Select(x=>x.ToString()).ToList() : [],
            }, urlHelper);
        
        return new SavedSearchViewModel(
            title,
            source.Id,
            source.SearchParameters.SearchTerm,
            source.SearchParameters.Distance,
            source.SearchParameters.SelectedRouteIds?.Select(category => routes.FirstOrDefault(route => route.Id == category)?.Name ?? string.Empty).ToList(),
            source.SearchParameters.SelectedLevelIds,
            source.SearchParameters.DisabilityConfident,
            source.SearchParameters.ExcludeNational ?? false,
            location,
            url,
            readOnly
        );
    }
}

public record SavedSearchesViewModel(
    List<SavedSearchViewModel> SavedSearches,
    int SavedSearchLimit,
    bool ShowDeletedBanner = false,
    string? DeletedSavedSearchTitle = null);