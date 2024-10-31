using Microsoft.CodeAnalysis.CSharp.Syntax;
using SFA.DAS.FAA.Application.Queries.SavedSearches;

namespace SFA.DAS.FAA.Web.Models.SavedSearches
{
    public class UnsubscribeSavedSearchesViewModel
    {
        public string? Where { get; set; }
        public long? Distance { get; set; }
        public List<string>? Categories { get; set; }
        public List<long>? Levels { get; set; }

        public static implicit operator UnsubscribeSavedSearchesViewModel?(GetConfirmUnsubscribeResult model)
        {
            if (model.SavedSearch == null) return null;

            return new UnsubscribeSavedSearchesViewModel()
            {
                Where = model.SavedSearch.Where,
                Distance = model.SavedSearch.Distance,
                Categories = model.SavedSearch.Categories,
                Levels = model.SavedSearch.Levels
            };
        }
    }
}
