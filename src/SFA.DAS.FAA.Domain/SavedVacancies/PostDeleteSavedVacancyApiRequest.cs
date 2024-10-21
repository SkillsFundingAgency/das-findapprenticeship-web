using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SavedVacancies
{
    public record PostDeleteSavedVacancyApiRequest(Guid CandidateId, PostDeleteSavedVacancyApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"saved-vacancies/{CandidateId}/delete";
        public object Data { get; set; } = Body;
    }

    public record PostDeleteSavedVacancyApiRequestData
    {
        public required string VacancyReference { get; set; }
    }
}