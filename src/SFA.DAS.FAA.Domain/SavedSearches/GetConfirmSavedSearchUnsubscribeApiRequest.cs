using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class GetConfirmSavedSearchUnsubscribeApiRequest : IGetApiRequest
    {
        private readonly Guid _search_id;

        public GetConfirmSavedSearchUnsubscribeApiRequest(Guid search_id)
        {
            _search_id = search_id;
        }
        public string GetUrl => $"saved-searches/{_search_id}/unsubscribe";
    }
}
