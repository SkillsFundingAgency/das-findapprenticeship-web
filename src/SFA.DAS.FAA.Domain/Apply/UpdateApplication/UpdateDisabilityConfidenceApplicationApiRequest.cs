using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateDisabilityConfidenceApplicationApiRequest
    (
    Guid ApplicationId,
    Guid CandidateId,
    UpdateDisabilityConfidenceApplicationModel UpdateApplicationModel)
    : IPostApiRequest
{
    public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/disability-confidence";

    public object Data { get; set; } = UpdateApplicationModel;
}
