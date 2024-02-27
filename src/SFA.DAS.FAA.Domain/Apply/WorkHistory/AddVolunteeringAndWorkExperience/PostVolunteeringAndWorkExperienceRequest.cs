using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience.PostVolunteeringAndWorkExperienceRequest;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience;

public class PostVolunteeringAndWorkExperienceRequest(Guid applicationId, PostVolunteeringAndWorkExperienceApiRequestData body)
    : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/volunteeringorworkexperience";
    public object Data { get; set; } = body;

    public record PostVolunteeringAndWorkExperienceApiRequestData
    {
        public Guid CandidateId { get; init; }
        public string? CompanyName { get; init; }
        public string? Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}