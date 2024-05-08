using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyApiRequest(string vacancyReference, string? candidateId) : IGetApiRequest
    {
        public string GetUrl => $"vacancies/{vacancyReference}?candidateId={candidateId}";
    }
}
