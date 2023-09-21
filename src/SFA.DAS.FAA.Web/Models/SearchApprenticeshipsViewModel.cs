using System.Xml.Schema;
using SFA.DAS.FAA.Application.Vacancies.Queries;

namespace SFA.DAS.FAA.Web.Models;

public class SearchApprenticeshipsViewModel
{
    public string? TotalText { get; set; }

    public static implicit operator SearchApprenticeshipsViewModel(GetSearchApprenticeshipsIndexResult source)
    {
        return new SearchApprenticeshipsViewModel
        {
            TotalText = $"{source.Total:N0} {(source.Total == 1 ? "apprenticeship" : "apprenticeships")} currently listed"
        };
    }

}

