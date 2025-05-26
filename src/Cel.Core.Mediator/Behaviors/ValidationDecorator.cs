using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Cel.Core.Mediator.Behaviors;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> innerHandler, IEnumerable<IValidator<TCommand>> validators)
        : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.Handle(command, cancellationToken);

            Error[] errors = CreateValidationError(validationFailures);

            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                return ReponseAsResultPattern(errors, responseType);

            throw new ValidationException("Validation failed", validationFailures);
        }

        private static TResponse ReponseAsResultPattern(Error[] errors, Type responseType)
        {
            Type innerType = responseType.GetGenericArguments()[0];

            // Call Result.Failure<innerType>(errors) via reflection
            var method = typeof(Result)
                .GetMethod(nameof(Result.Failures))
                ?.MakeGenericMethod(innerType);

            if (method == null)
                throw new InvalidOperationException("Result.Failure<T> method not found.");

            var result = method.Invoke(null, [errors]); // null for static method

            return (TResponse)result!;
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(TCommand command, IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
            return [];

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    private static Error[] CreateValidationError(ValidationFailure[] validationFailures)
        => validationFailures.Select(f => Error.Validation(f.ErrorMessage, f.ErrorCode)).ToArray();
}