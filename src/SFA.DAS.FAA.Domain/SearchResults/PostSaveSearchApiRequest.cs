using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class PostSaveSearchApiRequest(Guid candidateId, PostSaveSearchApiRequest.PostSaveSearchApiRequestData payload) : IPostApiRequest
{
    public string PostUrl => $"searchapprenticeships/saved-search?candidateId={candidateId}";
    public object Data { get; set; } = payload;

    public record PostSaveSearchApiRequestData(
        bool DisabilityConfident,
        int? Distance,
        string? Location,
        string? SearchTerm,
        List<string>? SelectedLevelIds,
        List<string>? SelectedRouteIds,
        string? SortOrder
    );
}