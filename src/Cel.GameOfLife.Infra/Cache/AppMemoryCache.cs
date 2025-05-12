using Cel.GameOfLife.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Cel.GameOfLife.Infra.Cache;

public class AppMemoryCache : IAppMemoryCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly HashSet<string> _keys;

    public AppMemoryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _keys = [];
    }

    public T? Get<T>(string key)
        => _memoryCache.TryGetValue(key, out T? value) ? value : default;

    public void Remove(string keyPattern)
    {
        List<string> keys = GetKeys(keyPattern).ToList();
        foreach (string k in keys)
        {
            _memoryCache.Remove(k);
            _keys.Remove(k);
        }
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        _memoryCache.Set(key, value, expiration);
        _keys.Add(key);
    }

    private IEnumerable<string> GetKeys(string value)
        => _keys.Where(key => key.Contains(value, StringComparison.OrdinalIgnoreCase));
}
