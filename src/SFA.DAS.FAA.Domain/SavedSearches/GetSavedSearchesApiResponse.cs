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
        List<int>? SelectedRouteIds,
        decimal? Distance,
        bool DisabilityConfident,
        List<int>? SelectedLevelIds,
        string? Location
    );
    
    public List<SavedSearchDto> SavedSearches { get; init; }
    public List<RouteResponse> Routes { get; init; }
}