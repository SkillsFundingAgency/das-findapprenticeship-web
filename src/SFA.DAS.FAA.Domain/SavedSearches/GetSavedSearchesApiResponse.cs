using SFA.DAS.FAA.Domain.BrowseByInterests;

namespace SFA.DAS.FAA.Domain.SavedSearches;

public class GetSavedSearchesApiResponse
{
    public record SavedSearchDto(
        Guid Id,
        DateTime DateCreated,
        DateTime? LastRunDate,
        DateTime? EmailLastSendDate,
        SearchParametersDto SearchParameters
    );

    public record SearchParametersDto(
        string? SearchTerm,
        List<string>? Categories,
        int? Distance,
        bool DisabilityConfident,
        List<string>? Levels,
        string? Location
    );
    
    public List<SavedSearchDto> SavedSearches { get; init; }
    public List<RouteResponse> Routes { get; init; }
}