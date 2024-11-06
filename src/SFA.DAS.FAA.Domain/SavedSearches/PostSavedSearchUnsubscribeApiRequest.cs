using SFA.DAS.FAA.Domain.Interfaces;
using System.Reflection;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class PostSavedSearchUnsubscribeApiRequest(Guid SavedSearchId) :IPostApiRequest
    {
        public string PostUrl => $"saved-searches/{SavedSearchId}/unsubscribe";
        public object Data { get; set; } = SavedSearchId;
    }
}
