using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.FAA.Web.Infrastructure
{
    public interface ICacheStorageService
    {
        /// <summary>
        /// Contract to get/retrieve the object value from MemoryCache by given CacheKey.
        /// </summary>
        /// <typeparam name="T">Object of type T</typeparam>
        /// <param name="key">CacheKey</param>
        /// <returns>Object of type T</returns>
        T? Get<T>(string key);

        /// <summary>
        /// Contract to store the object value in MemoryCache by given CacheKey.
        /// </summary>
        /// <typeparam name="T">Object of type T</typeparam>
        /// <param name="key">CacheKey</param>
        /// <param name="objectToCache">Object to Cache.</param>
        /// <param name="slidingExpiration">Timespan value in Minutes</param>
        /// <param name="absoluteExpiration">Timespan value in Minutes</param>
        /// <returns>Object of type T</returns>
        T? Set<T>(string key, T? objectToCache, int slidingExpiration = 30, int absoluteExpiration = 60);

        /// <summary>
        /// Contract to removes the object value from MemoryCache by given CacheKey.
        /// </summary>
        /// <param name="key">CacheKey</param>
        void Remove(string key);
    }

    public class CacheStorageService(IMemoryCache memoryCache) : ICacheStorageService
    {
        ///<inheritdoc/>
        public T? Get<T>(string key)
        {
            return memoryCache.TryGetValue(key, out T? objectFromCache) ? objectFromCache : default;
        }

        ///<inheritdoc/>
        public T? Set<T>(string key, T? objectToCache, int slidingExpiration = 30, int absoluteExpiration = 60)
        {
            return memoryCache.Set(key, objectToCache, new MemoryCacheEntryOptions
            {
                Size = 1024,
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration),
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(absoluteExpiration)
            });
        }

        ///<inheritdoc/>
        public void Remove(string key)
        {
            if (memoryCache.TryGetValue(key, out _))
            {
                memoryCache.Remove(key);
            }
        }
    }
}