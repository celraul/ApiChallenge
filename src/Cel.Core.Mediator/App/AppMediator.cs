using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Cel.Core.Mediator.App;

public class AppMediator(IServiceProvider serviceProvider) : IAppMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<Result<TResponse>> Send<TQuery, TResponse>(TQuery request, CancellationToken cancellationToken = default)
        where TQuery : ICommand<TResponse>
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider
                          .GetRequiredService<ICommandHandler<TQuery, TResponse>>();

        Result<TResponse> result = await handler.Handle(request, cancellationToken);

        return result;
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
         where TRequest : ICommand
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider
                          .GetRequiredService<ICommandHandler<TRequest>>();

        await handler.Handle(request, cancellationToken);
    }

    public async Task<Result<TResponse>> Query<TQuery, TResponse>(TQuery request, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider
                          .GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        Result<TResponse> result = await handler.Handle(request, cancellationToken);
        return result;
    }
}