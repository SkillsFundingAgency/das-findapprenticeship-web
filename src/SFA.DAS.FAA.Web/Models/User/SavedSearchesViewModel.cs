using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.User;

public class SavedSearchesViewModel
{
    public class SavedSearchViewModel
    {
        public Guid Id { get; init; }
        public string? SearchTerm { get; init; }
        public int? Distance { get; init; }
        public List<string> Categories { get; init; } = [];
        public List<string> Levels { get; init; } = [];
        public bool DisabilityConfident { get; init; }
        public string? Location { get; init; }

        public static SavedSearchViewModel From(SavedSearch source)
        {
            return new SavedSearchViewModel
            {
                Id = source.Id,
                SearchTerm = source.SearchParameters.SearchTerm,
                Distance = source.SearchParameters.Distance,
                Categories = source.SearchParameters.Categories,
                Levels = source.SearchParameters.Levels,
                DisabilityConfident = source.SearchParameters.DisabilityConfident,
                Location = source.SearchParameters.Location
            };
        }
    }

    public List<SavedSearchViewModel> SavedSearches { get; init; } = [];
}