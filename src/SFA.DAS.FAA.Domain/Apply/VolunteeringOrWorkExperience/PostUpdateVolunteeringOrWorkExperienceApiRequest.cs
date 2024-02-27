using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;

public record PostUpdateVolunteeringOrWorkExperienceApiRequest(Guid ApplicationId, Guid Id, PostUpdateVolunteeringOrWorkExperienceApiRequest.PostUpdateVolunteeringOrWorkExperienceApiRequestData Body)
    : IPostApiRequest
{
    public string PostUrl => $"applications/{ApplicationId}/volunteeringorworkexperience/{Id}";
    public object Data { get; set; } = Body;

    public record PostUpdateVolunteeringOrWorkExperienceApiRequestData
    {
        public Guid CandidateId { get; init; }
        public string? EmployerName { get; init; }
        public string? Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}