using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Vacancy
{
    public class PostApplyRequest
    {
        [FromRoute] public required string VacancyReference { get; set; }
    }
}
