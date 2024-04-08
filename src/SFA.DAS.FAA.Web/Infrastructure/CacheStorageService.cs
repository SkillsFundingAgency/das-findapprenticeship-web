using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.FAA.Web.Infrastructure
{
    public interface ICacheStorageService
    {
        T? Get<T>(string key);
        T? Set<T>(string key, T? objectToCache, TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration);
    }

    public class CacheStorageService(IMemoryCache memoryCache) : ICacheStorageService
    {
        public T? Get<T>(string key)
        {
            return memoryCache.TryGetValue(key, out T? objectFromCache) ? objectFromCache : default;
        }

        public T? Set<T>(string key, T? objectToCache, TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration)
        {
            return memoryCache.GetOrCreate(key, entry =>
            {
                entry.Size = 1024; //Size amount in Bytes
                entry.SetSlidingExpiration(slidingExpiration ?? TimeSpan.FromMinutes(30)); // Keep in cache for this time, reset time if accessed.
                entry.SetAbsoluteExpiration(absoluteExpiration ?? TimeSpan.FromHours(1)); // Remove from cache after this time, regardless of sliding expiration
                return objectToCache;
            });
        }
    }
}
