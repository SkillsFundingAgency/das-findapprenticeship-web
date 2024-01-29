using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory;

public class GetApplicationWorkHistoriesApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/{candidateId}/work-history";
}