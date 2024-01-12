using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetIndex
{
    public class GetIndexApiRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;

        public GetIndexApiRequest(string vacancyReference) => _vacancyReference = vacancyReference;

        public string GetUrl => $"vacancies/{_vacancyReference}/apply";
    }
}
