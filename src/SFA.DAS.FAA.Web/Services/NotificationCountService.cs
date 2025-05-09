using MediatR;
using SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Services
{
    public class NotificationCountService(
                IMediator mediator,
                ICacheStorageService cacheStorageService) : INotificationCountService
    {
        private const int SlidingExpirationMinutes = 30;
        private const int AbsoluteExpirationMinutes = 60;

        public async Task<int> GetUnreadApplicationCount(Guid candidateId, ApplicationStatus status)
        {
            await cacheStorageService.Remove(GenerateCacheKey(candidateId, status));
            var allNotifications = await mediator.Send(new GetApplicationsCountQuery(candidateId, status));
            var readIds = await GetCachedReadNotificationIdsAsync(candidateId, status);

            return allNotifications.Stats.ApplicationIds
                .Count(n => !readIds.Contains(n));
        }

        public async Task MarkAllNotificationsAsRead(Guid candidateId, ApplicationStatus status)
        {
            var result = await mediator.Send(new GetApplicationsCountQuery(candidateId, status));
            if (result.Stats.Count == 0) return;

            var cacheKey = GenerateCacheKey(candidateId, status);
            var readIds = result.Stats.ApplicationIds.ToList();
            await cacheStorageService.Set(cacheKey, readIds, SlidingExpirationMinutes, AbsoluteExpirationMinutes);
        }

        private async Task<List<Guid>> GetCachedReadNotificationIdsAsync(Guid candidateId, ApplicationStatus status)
        {
            var cacheKey = GenerateCacheKey(candidateId, status);
            return await cacheStorageService.Get<List<Guid>>(cacheKey) ?? [];
        }

        private static string GenerateCacheKey(Guid candidateId, ApplicationStatus status)
        {
            return $"{candidateId}-{status}-{CacheKeys.ApplicationNotificationsMarkAsRead}";
        }
    }
}
