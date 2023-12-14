using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyApiRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;

        public GetApprenticeshipVacancyApiRequest(string vacancyReference) => _vacancyReference = vacancyReference;

        public string GetUrl => $"vacancies/{_vacancyReference}";
    }
}
