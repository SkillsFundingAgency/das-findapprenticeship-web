using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetIndex
{
    public class GetIndexApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}?candidateId={candidateId}";
    }
}
