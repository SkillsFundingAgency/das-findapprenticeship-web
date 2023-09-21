using System.Xml.Schema;
using SFA.DAS.FAA.Application.Vacancies.Queries;

namespace SFA.DAS.FAA.Web.Models;

public class SearchApprenticeshipsViewModel
{
    public int Total { get; set; }
    public string TotalText { get; set; }

    public static implicit operator SearchApprenticeshipsViewModel(GetSearchApprenticeshipsIndexResult source)
    {
        string formattedTotal = source.Total.ToString("N0");
        string vacanciesText = source.Total == 1 ? "apprenticeship" : "apprenticeships";

        return new SearchApprenticeshipsViewModel
        {
            Total = source.Total,
            TotalText = $"{formattedTotal} {vacanciesText}"
        };
    }

}

