using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsApiRequest(Guid candidateId, Guid applicationId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/skillsandstrengths/candidate?candidateId={candidateId}";

}
