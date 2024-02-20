using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
public class GetVolunteeringOrWorkExperienceItemApiRequest(Guid ApplicationId, Guid Id, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/volunteeringorworkexperience/{Id}?candidateId={CandidateId}";
}
