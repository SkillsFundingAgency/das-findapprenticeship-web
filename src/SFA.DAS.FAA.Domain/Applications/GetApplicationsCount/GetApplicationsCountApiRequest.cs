using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.GetApplicationsCount
{
    public record GetApplicationsCountApiRequest(Guid CandidateId, ApplicationStatus Status) : IGetApiRequest
    {
        public string GetUrl => $"applications/count?candidateId={CandidateId}&status={Status}";
    }
}