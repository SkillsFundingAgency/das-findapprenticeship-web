using SFA.DAS.FAA.Domain.Interfaces;
using System.Reflection;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class PostSavedSearchUnsubscribeApiRequest(Guid savedSearchId) :IPostApiRequest
    {
        public string PostUrl => $"saved-searches/unsubscribe";
        public object Data { get; set; } = new PostSavedSearchUnsubscribeApiRequestData{ SavedSearchId = savedSearchId};
    }

    public class PostSavedSearchUnsubscribeApiRequestData
    {
        public Guid SavedSearchId { get; set; }
    }
}
