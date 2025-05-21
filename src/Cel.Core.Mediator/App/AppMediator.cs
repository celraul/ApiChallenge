using Cel.Core.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cel.Core.Mediator.App;

public class AppMediator(IServiceProvider serviceProvider) : IAppMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var commandType = request.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        TResponse result = await handler.Handle((dynamic)request, cancellationToken);

        return result;
    }

    public async Task Send(ICommand request, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var commandType = request.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        await handler.Handle((dynamic)request, cancellationToken);
    }

    public async Task<TResponse> Query<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var commandType = request.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

        TResponse result = await handler.Handle((dynamic)request, cancellationToken);
        return result;
    }
}