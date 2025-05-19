using Microsoft.Extensions.Caching.Memory;

namespace ServerSideStateManagementApp.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan expirationTime)
        {
            if (!_cache.TryGetValue(key, out T cacheEntry))
            {
                cacheEntry = createItem();

                var cacheEntryOptions = new MemoryCacheEntryOptions{ AbsoluteExpirationRelativeToNow = expirationTime};

                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }
    }
}