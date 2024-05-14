using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.SubmitPreviewApplication;

public class SubmitPreviewApplicationRequest(Guid candidateId, Guid applicationId) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/preview?candidateId={candidateId}";
    public object Data { get; set; }
}