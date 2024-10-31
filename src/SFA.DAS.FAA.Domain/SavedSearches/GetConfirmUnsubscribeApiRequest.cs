using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedSearches
{
    public class GetConfirmUnsubscribeApiRequest : IGetApiRequest
    {
        private readonly string? _search_id;

        public GetConfirmUnsubscribeApiRequest(string? search_id)
        {
            _search_id = search_id;
        }
        public string GetUrl => $"savedsearches?searchId={HttpUtility.UrlEncode(_search_id)}/unsubscribe";
    }
}
