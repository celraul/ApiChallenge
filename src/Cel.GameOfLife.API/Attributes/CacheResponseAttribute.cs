namespace Cel.GameOfLife.API.Attributes;

public class CacheResponseAttribute(int cacheDurationInSeconds = 60) : Attribute
{
    public int CacheDurationInSeconds { get; } = cacheDurationInSeconds;
}
