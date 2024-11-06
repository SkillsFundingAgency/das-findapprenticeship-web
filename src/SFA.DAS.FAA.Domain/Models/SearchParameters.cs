namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<string> Categories,
    int? Distance,
    bool DisabilityConfident,
    List<string> Levels,
    string? Location
);