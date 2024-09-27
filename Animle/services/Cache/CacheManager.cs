using Animle.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Animle.Services
{


    public class RequestCacheManager : IRequestCacheManager
    {
        private readonly MemoryCache _cache;

        public RequestCacheManager()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<T> GetCachedOrRequest<T>(string cacheKey, Func<Task<T>> request, TimeSpan cacheDuration)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedData))
            {
                return cachedData;
            }

            T requestData = await request.Invoke();

            if (requestData != null)
            {
                _cache.Set(cacheKey, requestData, DateTimeOffset.Now.Add(cacheDuration));
            }

            return requestData;
        }

        public void SetCacheItem<T>(string cacheKey, T item, TimeSpan cacheDuration)
        {
            _cache.Set(cacheKey, item, DateTimeOffset.Now.Add(cacheDuration));
        }

        public T? GetCachedItem<T>(string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedData))
            {
                return cachedData;
            }

            return default;
        }
    }
}
