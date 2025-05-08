using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Applications.GetApplicationsCount.GetApplicationsCountApiRequest;

namespace SFA.DAS.FAA.Domain.Applications.GetApplicationsCount
{
    public record GetApplicationsCountApiRequest(Guid CandidateId, GetApplicationsCountApiRequestData Payload) : IPostApiRequest
    {
        public object Data { get; set; } = Payload.Statuses;
        public string PostUrl => $"applications/count?candidateId={CandidateId}";

        public record GetApplicationsCountApiRequestData(List<ApplicationStatus> Statuses)
        {
            public List<ApplicationStatus> Statuses { get; } = Statuses;
        }
    }
}
