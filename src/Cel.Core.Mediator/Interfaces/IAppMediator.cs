using Cel.Core.Mediator.Models;

namespace Cel.Core.Mediator.Interfaces;

public interface IAppMediator
{
    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
    Task Send(ICommand request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);
}
