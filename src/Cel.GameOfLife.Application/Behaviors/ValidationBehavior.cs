using FluentValidation;
using MediatR;

namespace Cel.GameOfLife.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellation)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(a => a.ValidateAsync(context, cancellation)));

            var failures = validationResults.SelectMany(x => x.Errors).Where(f => f != null).ToList();
            if (failures.Any())
                throw new Domain.Exceptions.ValidationException(failures.Select(x => x.ErrorMessage).ToList());
        }

        return await next();
    }
}
