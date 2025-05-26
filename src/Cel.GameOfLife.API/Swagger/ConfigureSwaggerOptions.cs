using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Cel.GameOfLife.API.Swagger;

internal class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {

        options.ExampleFilters();
        IncludeDocumentationComments(options);

        // add swagger document for every API version discovered
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        OpenApiInfo info = new()
        {
            Title = DefaultDocsInfo.AppTitle,
            Version = description.ApiVersion.ToString(),
            Description = $"Generated at {DateTime.Now:HH:mm:ss dd/MM/yyyy}",
            Contact = new OpenApiContact()
            {
                Name = DefaultDocsInfo.ContactName,
                Email = DefaultDocsInfo.ContactEmail,
            },
            Extensions = new Dictionary<string, IOpenApiExtension>
            {
                { "x-logo", new OpenApiObject
                    {
                        {
                            "url",
                            new OpenApiString("https://i.postimg.cc/xTwb0Lf5/whtf-logo-transparent-1.png")
                        },
                        {
                            "altText",
                            new OpenApiString("Whtf")
                        }
                    }
                }
            }
        };

        if (description.IsDeprecated)
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";

        return info;
    }

    private static void IncludeDocumentationComments(SwaggerGenOptions swaggerGenOptions)
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        swaggerGenOptions.IncludeXmlComments(xmlPath);
    }
}
