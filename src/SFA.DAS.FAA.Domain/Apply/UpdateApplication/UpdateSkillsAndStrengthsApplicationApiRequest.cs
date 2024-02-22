using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateSkillsAndStrengthsApplicationApiRequest
(
    Guid ApplicationId,
    Guid CandidateId,
    UpdateSkillsAndStrengthsApplicationModel UpdateApplicationModel)
    : IPostApiRequest
{
    public object Data { get; set; } = UpdateApplicationModel;

    public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/skillsandstrengths";
}
