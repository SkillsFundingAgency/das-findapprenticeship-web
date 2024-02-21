using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;
public class GetSkillsAndStrengthsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/skillsandstrengths?candidateId={candidateId}";
}
