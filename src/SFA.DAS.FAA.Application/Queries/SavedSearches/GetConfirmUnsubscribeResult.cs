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
        public Guid Id { get; set; }
        public string? SearchTitle { get; set; }
        public string? What { get; set; }
        public string? Where { get; set; }
        public long? Distance { get; set; }
        public List<string>? Categories { get; set; }
        public List<long>? Levels { get; set; }
        public bool DisabilityConfident { get; set; } = false;
    }
}
