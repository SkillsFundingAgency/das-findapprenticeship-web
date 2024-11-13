using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedSearches;

public class GetSavedSearchApiRequest(Guid candidateId, Guid id) : IGetApiRequest
{
    public string GetUrl => $"users/{candidateId}/saved-searches/{id}";
}