using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.DeleteApplication;

public class PostDeleteApplicationApiRequest(Guid applicationId, Guid candidateId): IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/delete?candidateId={candidateId}";
    public object Data { get; set; }
}