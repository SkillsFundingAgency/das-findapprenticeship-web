using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Web.Models;

public class SearchApprenticeshipsViewModel
{
    public string? TotalText { get; set; }
    public string? WhereSearchTerm { get; set; }
    public int? Distance { get; set; } = 10;

    public static implicit operator SearchApprenticeshipsViewModel(GetSearchApprenticeshipsIndexResult source)
    {
        return new SearchApprenticeshipsViewModel
        {
            TotalText = $"{source.Total:N0} {(source.Total == 1 ? "apprenticeship" : "apprenticeships")} currently listed"
        };
    }

}

