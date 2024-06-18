using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public record ApplicationSubmittedVacancyInfo
    {
        public string? VacancyTitle { get; init; }
        public string? EmployerName { get; init; }
        public bool HasAnsweredEqualityQuestions { get; set; }

        public static implicit operator ApplicationSubmittedVacancyInfo(GetApplicationSubmittedQueryResponse source)
        {
            return new ApplicationSubmittedVacancyInfo
            {
                EmployerName = source.EmployerName,
                VacancyTitle = source.VacancyTitle,
                HasAnsweredEqualityQuestions = source.HasAnsweredEqualityQuestions
            };
        }
    }
}
