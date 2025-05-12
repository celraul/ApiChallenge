namespace Cel.GameOfLife.Application.Interfaces;

public interface IAppMemoryCache
{
    T? Get<T>(string key);
    void Remove(string key);
    void Set<T>(string key, T value, TimeSpan expiration);
}
