using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateApplicationStatusApiRequest
(
    Guid ApplicationId,
    Guid CandidateId,
    UpdateApplicationStatusModel UpdateApplicationStatusModel)
    : IPostApiRequest
{
    public object Data { get; set; } = UpdateApplicationStatusModel;

    public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/status";
}

public record UpdateApplicationStatusModel
{
    public ApplicationStatus Status { get; set; }
}