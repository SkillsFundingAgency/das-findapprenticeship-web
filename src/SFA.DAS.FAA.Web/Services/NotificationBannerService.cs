using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Services
{
    public static class NotificationBannerService
    {
        public static async Task<bool> ShowAccountCreatedBanner(ICacheStorageService cacheStorageService, string key)
        {
            var showBanner = await cacheStorageService.Get<bool>(key);

            if (showBanner)
            {
                await cacheStorageService.Remove(key);
                return showBanner;
            }

            return false;
        }
    }
}
