namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    decimal? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location
);