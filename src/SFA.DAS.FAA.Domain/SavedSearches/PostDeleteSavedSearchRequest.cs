using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedSearches;

public class PostDeleteSavedSearchRequest(Guid candidateId, PostDeleteSavedSearchRequest.PostDeleteSavedSearchRequestData payload) : IPostApiRequest
{
    public string PostUrl { get; } = $"users/{candidateId}/saved-searches/delete";
    public object Data { get; set; } = payload;
    
    public record PostDeleteSavedSearchRequestData(Guid Id);
}