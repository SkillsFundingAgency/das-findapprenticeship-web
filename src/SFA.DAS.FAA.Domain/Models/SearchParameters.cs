namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location
);