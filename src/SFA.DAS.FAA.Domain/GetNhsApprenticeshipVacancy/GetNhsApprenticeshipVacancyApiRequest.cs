using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.GetNhsApprenticeshipVacancy
{
    public class GetNhsApprenticeshipVacancyApiRequest(string vacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"vacancies/nhs/{vacancyReference}";
    }
}