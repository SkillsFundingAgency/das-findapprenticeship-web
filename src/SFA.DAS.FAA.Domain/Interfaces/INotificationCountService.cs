using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface INotificationCountService
    {
        Task<List<ApplicationStatusCount>> GetTotalApplicationsStatusCount(Guid candidateId,
            List<ApplicationStatus> statuses);
        Task<ApplicationStatusCount> GetApplicationsStatusCount(Guid candidateId, ApplicationStatus status);
    }
}
