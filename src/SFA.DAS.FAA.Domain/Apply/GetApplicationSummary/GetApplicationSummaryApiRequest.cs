using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;

public class GetApplicationSummaryApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/{candidateId}/summary";
}
