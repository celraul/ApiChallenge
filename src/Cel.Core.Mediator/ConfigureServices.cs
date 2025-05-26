using Cel.Core.Mediator.Behaviors;
using Cel.Core.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Cel.Core.Mediator.App;

namespace Cel.Core.Mediator;

public static class ConfigureServices
{
    public static IServiceCollection AddAppMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddDefaultHandlers(assembly)
            .AddPipelineHandlers()
            .AddDomainHandlers(assembly)
            .AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddDomainHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDefaultHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddPipelineHandlers(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        services.AddSingleton(typeof(IAppMediator), typeof(AppMediator));

        return services;
    }
}
