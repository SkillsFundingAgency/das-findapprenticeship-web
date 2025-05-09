using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface INotificationCountService
    {
        Task<int> GetUnreadApplicationCount(Guid candidateId, ApplicationStatus status);
        Task MarkAllNotificationsAsRead(Guid candidateId, ApplicationStatus status);
    }
}