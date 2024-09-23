using Microsoft.Extensions.Caching.Memory;
namespace Animle.services.Cache
{

    public class RequestCacheManager
    {
        private MemoryCache _cache;

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
        public void SetCacheItem<T>(string cacheKey, T Item, TimeSpan cacheDuration)
        {

            _cache.Set(cacheKey, Item, DateTimeOffset.Now.Add(cacheDuration));

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