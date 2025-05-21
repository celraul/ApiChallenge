using Cel.GameOfLife.API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Filters;

namespace Cel.GameOfLife.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, string defaultDisplayApiVerstion = "")
    {
        services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = string.IsNullOrEmpty(defaultDisplayApiVerstion) ? ApiVersion.Default : ApiVersion.Parse(defaultDisplayApiVerstion);
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ReportApiVersions = true;
            //o.ApiVersionReader = new HeaderApiVersionReader(DefaultDocsInfo.VersionNameField);
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // Example: "v1", "v2"
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(); // Using ConfigureSwaggerOptions
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddSwaggerExamplesFromAssemblyOf<Program>();

        return services;
    }

    public static void ConfigureSwaggerUI(this WebApplication app, string appName = "App")
    {
        app.UseSwagger();

        IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
       {
           foreach (ApiVersionDescription apiDescriptionGroup in apiVersionDescriptionProvider.ApiVersionDescriptions)
           {
               options.SwaggerEndpoint($"/swagger/{apiDescriptionGroup.GroupName}/swagger.json",
                    $"{appName} V{apiDescriptionGroup.ApiVersion}");
           }
       });

        app.UseReDoc(options =>
        {
            foreach (ApiVersionDescription apiDescriptionGroup in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.DocumentTitle = $"{appName} V{apiDescriptionGroup.ApiVersion}";
                options.SpecUrl = $"/swagger/{apiDescriptionGroup.GroupName}/swagger.json";
            }
        });
    }
}
