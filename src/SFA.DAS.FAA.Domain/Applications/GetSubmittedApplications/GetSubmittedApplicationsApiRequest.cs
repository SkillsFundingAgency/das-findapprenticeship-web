using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsApiRequest(Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/submitted?candidateId={CandidateId}";
    }
}