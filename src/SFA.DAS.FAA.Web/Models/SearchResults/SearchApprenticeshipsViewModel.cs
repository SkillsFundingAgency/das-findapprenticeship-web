using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Models.SearchResults;

public class SearchApprenticeshipsViewModel
{
    public string? TotalText { get; set; }
    public string? WhereSearchTerm { get; set; }
    public string? WhatSearchTerm { get; set; }
    public int? Distance { get; set; } = 10;
    public bool ShowAccountCreatedBanner { get; set; } = false;
    public bool ShowAccountFoundBanner { get; set; } = false;
    public bool ShowAccountDeletedBanner { get; set; } = false;

    public List<SavedSearchViewModel> SavedSearches { get; set; } = new List<SavedSearchViewModel>();
    
    public static implicit operator SearchApprenticeshipsViewModel(GetSearchApprenticeshipsIndexResult source)
    {
        return new SearchApprenticeshipsViewModel
        {
            
            TotalText = $"{source.Total:N0} {(source.Total == 1 ? "apprenticeship" : "apprenticeships")} currently listed"
        };
    }
}