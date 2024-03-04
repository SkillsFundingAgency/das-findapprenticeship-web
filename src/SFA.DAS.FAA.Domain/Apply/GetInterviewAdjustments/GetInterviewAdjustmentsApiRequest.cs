using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/interview-adjustments?candidateId={candidateId}";
}
