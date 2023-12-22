using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.Models
{
    public class ChecklistDetails
    {
        public string? Title { get; set; }
        public string? QueryStringParameterName { get; set; }

        public IEnumerable<ChecklistLookup> Lookups { get; set; } = Enumerable.Empty<ChecklistLookup>();
    }
}
