using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches
{
    public class GetConfirmUnsubscribeResult
    {
        public string Where { get; set; }
        public long Distance { get; set; }
        public string[] Categories { get; set; }
        public long[] Levels { get; set; }
    }
}
