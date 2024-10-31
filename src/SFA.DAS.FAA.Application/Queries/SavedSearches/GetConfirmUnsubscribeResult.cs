using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches
{
    public class GetConfirmUnsubscribeResult
    {
        public SavedSearch? SavedSearch { get; set; }
    }

    public class SavedSearch
    {
        public string? Where { get; set; }
        public long? Distance { get; set; }
        public List<string>? Categories { get; set; }
        public List<long>? Levels { get; set; }
    }
}
