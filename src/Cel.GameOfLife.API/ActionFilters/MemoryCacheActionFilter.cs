using Cel.GameOfLife.API.Attributes;
using Cel.GameOfLife.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Cel.GameOfLife.API.ActionFilters;

public class MemoryCacheActionFilter(IAppMemoryCache appMemoryCache) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        Endpoint? endpoint = httpContext.GetEndpoint();
        var cacheAtrtibute = endpoint?.Metadata.GetMetadata<CacheResponseAttribute>();

        if (httpContext.Request.Method != "GET" || cacheAtrtibute is null)
        {
            await next();
            return;
        }

        string key = GenerateKey(httpContext.Request);
        IActionResult? cachedResponse = appMemoryCache.Get<IActionResult>(key);
        if (cachedResponse is not null)
        {
            context.Result = cachedResponse;
            return;
        }

        var executionResult = await next();
        if (executionResult.Result is ObjectResult objectResult)
            appMemoryCache.Set(key, executionResult.Result, TimeSpan.FromSeconds(cacheAtrtibute.CacheDurationInSeconds));
    }

    private string GenerateKey(HttpRequest httpRequest)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"MemoryCache:{httpRequest.Path}:");

        foreach (var item in httpRequest.Query.OrderBy(q => q.Key))
            keyBuilder.Append($"-{item.Key}:{item.Value}");

        return keyBuilder.ToString();
    }
}

