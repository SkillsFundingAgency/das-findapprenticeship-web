using MediatR;
using SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Services
{
    public class NotificationCountService(
            IMediator mediator,
            ICacheStorageService cacheStorageService) : INotificationCountService
    {
        private const int SlidingExpirationMinutes = 30;
        private const int AbsoluteExpirationMinutes = 60;

        public async Task<List<ApplicationStatusCount>> GetTotalApplicationsStatusCount(Guid candidateId, List<ApplicationStatus> statuses)
        {
            var cacheKey = $"{candidateId}-{CacheKeys.TotalApplicationsNotificationCount}";
            var cachedResult = await cacheStorageService.Get<List<ApplicationStatusCount>>(cacheKey);
            if (cachedResult != null) return cachedResult;

            var result = await mediator.Send(new GetApplicationsCountQuery(candidateId, statuses));
            await cacheStorageService.Set(cacheKey, result.Stats, SlidingExpirationMinutes, AbsoluteExpirationMinutes);
            return result.Stats;
        }

        public async Task<ApplicationStatusCount> GetApplicationsStatusCount(Guid candidateId, ApplicationStatus status)
        {
            var cacheKey = $"{candidateId}-{status}-{CacheKeys.ApplicationStatusCount}";
            var cachedResult = await cacheStorageService.Get<ApplicationStatusCount>(cacheKey);
            if (cachedResult != null) return cachedResult;

            var result = await mediator.Send(new GetApplicationsCountQuery(candidateId, [status]));
            await cacheStorageService.Set(cacheKey, result, SlidingExpirationMinutes, AbsoluteExpirationMinutes);
            return result.Stats.FirstOrDefault(x => x.Status == status) ?? new ApplicationStatusCount([], 0, status);
        }
    }
}
