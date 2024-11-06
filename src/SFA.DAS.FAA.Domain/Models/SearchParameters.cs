namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<string>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    List<string>? SelectedLevelIds,
    string? Location
);