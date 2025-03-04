using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class PostSaveSearchApiRequest(Guid candidateId, Guid id, PostSaveSearchApiRequest.PostSaveSearchApiRequestData payload) : IPostApiRequest
{
    public string PostUrl => $"searchapprenticeships/saved-search?candidateId={candidateId}&id={id}";
    public object Data { get; set; } = payload;

    public record PostSaveSearchApiRequestData(
        bool DisabilityConfident,
        int? Distance,
        bool? ExcludeNational,
        string? Location,
        string? SearchTerm,
        List<string>? SelectedLevelIds,
        List<string>? SelectedRouteIds,
        string? SortOrder,
        string UnSubscribeToken,
        bool ExcludeNational
    );
}