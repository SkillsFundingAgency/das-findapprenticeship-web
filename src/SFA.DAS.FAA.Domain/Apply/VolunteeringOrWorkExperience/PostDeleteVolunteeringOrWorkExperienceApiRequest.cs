using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
public class PostDeleteVolunteeringOrWorkExperienceApiRequest(Guid applicationId, Guid id, PostDeleteVolunteeringOrWorkExperienceApiRequest.PostDeleteVolunteeringOrWorkExperienceApiRequestData body) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/volunteeringorworkexperience/{id}/delete";
    public object Data { get; set; } = body;

    public class PostDeleteVolunteeringOrWorkExperienceApiRequestData
    {
        public Guid CandidateId { get; set; }

    }
}
