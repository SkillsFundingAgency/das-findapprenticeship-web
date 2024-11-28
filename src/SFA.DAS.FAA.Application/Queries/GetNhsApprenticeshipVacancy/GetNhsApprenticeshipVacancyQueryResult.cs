using SFA.DAS.FAA.Domain.GetNhsApprenticeshipVacancy;

namespace SFA.DAS.FAA.Application.Queries.GetNhsApprenticeshipVacancy
{
    public record GetNhsApprenticeshipVacancyQueryResult
    {
        public GetNhsApprenticeshipVacancyApiResponse? Vacancy { get; set; }
    }
}