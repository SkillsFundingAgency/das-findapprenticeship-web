using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.FAA.Application.Queries.SavedSearches;

namespace SFA.DAS.FAA.Web.Models.SavedSearches
{
    public class UnsubscribeSavedSearchesViewModel
    {
        public Guid Id { get; set; }
        public string? SearchTitle { get; set; }
        public string? What {  get; set; }
        public string? Where { get; set; }
        public long? Distance { get; set; }
        public List<string>? Categories { get; set; }
        public List<long>? Levels { get; set; }
        public bool DisabilityConfident { get; set; }

        public static implicit operator UnsubscribeSavedSearchesViewModel?(GetConfirmUnsubscribeResult model)
        {
            if (model.SavedSearch == null) return null;

            return new UnsubscribeSavedSearchesViewModel()
            {
                Id = model.SavedSearch.Id,
                SearchTitle = model.SavedSearch.SearchTitle,
                What = model.SavedSearch.What,
                Where = model.SavedSearch.Where,
                Distance = model.SavedSearch.Distance,
                Categories = model.SavedSearch.Categories,
                Levels = model.SavedSearch.Levels,
                DisabilityConfident = model.SavedSearch.DisabilityConfident
            };
        }
    }
}
