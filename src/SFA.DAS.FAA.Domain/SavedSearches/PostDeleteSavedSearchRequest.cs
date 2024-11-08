using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedSearches;

public class PostDeleteSavedSearchRequest(Guid candidateId, Guid id) : IPostApiRequest
{
    public string PostUrl { get; } = $"users/{candidateId}/saved-searches/{id}/delete";
    public object Data { get; set; }
}