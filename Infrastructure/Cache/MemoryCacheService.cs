using Microsoft.Extensions.Caching.Memory;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory = null);
    void Set<T>(string key, T value);
    void Remove(string key);
}

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory = null)
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        if (factory != null)
        {
            value = await factory();
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(1));
            return value;
        }
        return default;
    }

    public void Set<T>(string key, T value)
    {
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(1));
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}
