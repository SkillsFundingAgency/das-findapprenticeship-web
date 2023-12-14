using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Vacancy
{
    public class GetVacancyDetailsRequest
    {
        [FromRoute] public required string VacancyReference { get; init; }
    }
}
