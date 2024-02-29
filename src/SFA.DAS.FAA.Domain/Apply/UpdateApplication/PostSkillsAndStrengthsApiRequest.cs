using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record PostSkillsAndStrengthsApiRequest(Guid ApplicationId, PostSkillsAndStrengthsModel UpdateApplicationModel)
    : IPostApiRequest
{
    public object Data { get; set; } = UpdateApplicationModel;

    public string PostUrl => $"applications/{ApplicationId}/skillsandstrengths";
}
