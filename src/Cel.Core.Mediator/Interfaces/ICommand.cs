namespace Cel.Core.Mediator.Interfaces;

public interface ICommand : IRequest;

public interface ICommand<TResponse> : IRequest<TResponse>;
