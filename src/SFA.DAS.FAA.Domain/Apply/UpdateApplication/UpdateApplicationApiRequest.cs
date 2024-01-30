using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication
{
    public record UpdateApplicationApiRequest(
        Guid ApplicationId,
        Guid CandidateId,
        UpdateApplicationModel UpdateApplicationModel)
        : IPostApiRequest
    {
        public object Data { get; set; } = UpdateApplicationModel;

        public string PostUrl => $"applications/{ApplicationId}/{CandidateId}";
    }
}