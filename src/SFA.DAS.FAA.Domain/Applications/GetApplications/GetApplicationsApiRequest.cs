using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.GetApplications
{
    public class GetApplicationsApiRequest(Guid candidateId, ApplicationStatus status) : IGetApiRequest
    {
        public string GetUrl => $"applications?candidateId={candidateId}&status={status}";
    }
}
