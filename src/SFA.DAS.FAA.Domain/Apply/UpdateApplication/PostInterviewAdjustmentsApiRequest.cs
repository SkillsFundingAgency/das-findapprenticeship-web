using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record PostInterviewAdjustmentsApiRequest(Guid ApplicationId, PostInterviewAdjustmentsModel UpdateApplicationModel)
        : IPostApiRequest
{
    public object Data { get; set; } = UpdateApplicationModel;

    public string PostUrl => $"applications/{ApplicationId}/interviewadjustments";
}
