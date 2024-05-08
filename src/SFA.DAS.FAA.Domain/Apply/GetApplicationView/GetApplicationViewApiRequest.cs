using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetApplicationView
{
    public record GetApplicationViewApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/{CandidateId}/view";
    }
}