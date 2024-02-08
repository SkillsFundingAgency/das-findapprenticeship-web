using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication
{
    public record UpdateWorkHistoryApplicationApiRequest(
        Guid ApplicationId,
        Guid CandidateId,
        UpdateWorkHistoryApplicationModel UpdateApplicationModel)
        : IPostApiRequest
    {
        public object Data { get; set; } = UpdateApplicationModel;

        public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/work-history";
    }
}