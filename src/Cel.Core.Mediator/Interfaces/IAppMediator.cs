using Cel.Core.Mediator.Models;

namespace Cel.Core.Mediator.Interfaces;

public interface IAppMediator
{
    Task<Result<TResponse>> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : ICommand<TResponse>;

    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : ICommand;
    Task<Result<TResponse>> Query<TQuery, TResponse>(TQuery request, CancellationToken cancellationToken = default) where TQuery : IQuery<TResponse>;
}
