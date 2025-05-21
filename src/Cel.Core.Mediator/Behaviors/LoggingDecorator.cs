using Cel.Core.Mediator.Interfaces;

namespace Cel.Core.Mediator.Behaviors;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> innerHandler)
        : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;
            TResponse result = await innerHandler.Handle(command, cancellationToken);

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(IQueryHandler<TQuery, TResponse> innerHandler) : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
        {
            string queryName = typeof(TQuery).Name;

            TResponse result = await innerHandler.Handle(query, cancellationToken);

            return result;
        }
    }
}
