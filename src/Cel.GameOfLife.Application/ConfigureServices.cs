using Cel.GameOfLife.Application.Behaviors;
using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cel.GameOfLife.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        services.AddMediator(assembly)
             .AddLayerServices();

        return services;
    }

    private static IServiceCollection AddLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IGameOfLifeService, GameOfLifeService>();

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
