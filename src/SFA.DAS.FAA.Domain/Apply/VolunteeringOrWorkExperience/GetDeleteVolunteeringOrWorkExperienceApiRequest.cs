using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceApiRequest(Guid ApplicationId, Guid Id, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/volunteeringorworkexperience/{Id}/delete?candidateId={CandidateId}";
}
