using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedVacancies
{
    public record PostSaveVacancyApiRequest(Guid CandidateId, PostSaveVacancyApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"saved-vacancies/{CandidateId}/add";
        public object Data { get; set; } = Body;
    }

    public record PostSaveVacancyApiRequestData
    {
        public required string VacancyReference { get; set; }
        public required string VacancyId { get; set; }
    }

    public record PostSaveVacancyApiResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}