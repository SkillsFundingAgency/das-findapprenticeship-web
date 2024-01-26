using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Vacancies.VacancyDetails.PostApplicationDetailsApiRequest;

namespace SFA.DAS.FAA.Domain.Vacancies.VacancyDetails
{
    public class PostApplicationDetailsApiRequest(Guid candidateId, string vacancyReference, RequestBody data) : IPostApiRequest
    {
        public string PostUrl => $"vacancies/{vacancyReference}";

        public object Data { get; set; } = data;

        public class RequestBody(Guid candidateId)
        {
            public Guid CandidateId { get; private set; } = candidateId;
        }
    }

    public class PostApplicationDetailsApiResponse
    {
        public Guid ApplicationId { get; set; }
    }
}
