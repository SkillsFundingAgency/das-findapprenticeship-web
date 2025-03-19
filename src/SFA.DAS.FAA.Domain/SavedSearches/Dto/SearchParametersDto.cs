namespace SFA.DAS.FAA.Domain.SavedSearches.Dto;

public record SearchParametersDto(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    bool? ExcludeNational,
    List<int>? SelectedLevelIds,
    string? Location
);