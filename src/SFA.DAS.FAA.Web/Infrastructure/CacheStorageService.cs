using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
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
        Task<T?> Get<T>(string key);

        /// <summary>
        /// Contract to store the object value in MemoryCache by given CacheKey.
        /// </summary>
        /// <typeparam name="T">Object of type T</typeparam>
        /// <param name="key">CacheKey</param>
        /// <param name="objectToCache">Object to Cache.</param>
        /// <param name="slidingExpiration">Timespan value in Minutes</param>
        /// <param name="absoluteExpiration">Timespan value in Minutes</param>
        /// <returns>Object of type T</returns>
        Task Set<T>(string key, T? objectToCache, int slidingExpiration = 30, int absoluteExpiration = 60);

        /// <summary>
        /// Contract to removes the object value from MemoryCache by given CacheKey.
        /// </summary>
        /// <param name="key">CacheKey</param>
        Task Remove(string key);
    }

    public class CacheStorageService(IDistributedCache distributedCache) : ICacheStorageService
    {
        ///<inheritdoc/>
        public async Task<T?> Get<T>(string key)
        {
            var json = await distributedCache.GetStringAsync(key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions{});
        }

        ///<inheritdoc/>
        public async Task Set<T>(string key, T? objectToCache, int slidingExpiration = 30, int absoluteExpiration = 60)
        {
            var json = JsonSerializer.Serialize(objectToCache);

            await distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpiration)
            });
        }

        ///<inheritdoc/>
        public async Task Remove(string key)
        {
            if (await distributedCache.GetAsync(key) != null)
            {
                distributedCache.Remove(key);
            }
        }
    }
}