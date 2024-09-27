namespace Animle.Interfaces
{
    public interface IRequestCacheManager
    {
        Task<T> GetCachedOrRequest<T>(string cacheKey, Func<Task<T>> request, TimeSpan cacheDuration);
        void SetCacheItem<T>(string cacheKey, T item, TimeSpan cacheDuration);
        T? GetCachedItem<T>(string cacheKey);
    }
}
