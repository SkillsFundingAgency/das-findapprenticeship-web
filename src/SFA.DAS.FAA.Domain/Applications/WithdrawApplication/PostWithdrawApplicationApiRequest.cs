using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.WithdrawApplication;

public class PostWithdrawApplicationApiRequest(Guid applicationId, Guid candidateId) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/withdraw?candidateId={candidateId}";
    public object Data { get; set; }
}