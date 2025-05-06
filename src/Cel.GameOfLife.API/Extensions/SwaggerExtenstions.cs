using Swashbuckle.AspNetCore.Filters;

namespace Cel.GameOfLife.API.Extensions;

public static class SwaggerExtenstions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
        });
        services.AddSwaggerExamplesFromAssemblyOf<Program>();

        return services;
    }
}
