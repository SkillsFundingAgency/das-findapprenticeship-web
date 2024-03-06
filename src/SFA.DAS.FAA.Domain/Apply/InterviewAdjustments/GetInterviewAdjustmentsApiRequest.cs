using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.InterviewAdjustments
{
    public class GetInterviewAdjustmentsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}/InterviewAdjustments?candidateId={candidateId}";
    }
}
