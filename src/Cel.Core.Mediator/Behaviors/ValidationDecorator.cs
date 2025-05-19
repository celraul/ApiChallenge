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
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.Handle(command, cancellationToken);

            ValidationError validationError = CreateValidationError(validationFailures);

            return Result.Failure<TResponse>(validationError.Errors.ToList());
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

    private static ValidationError CreateValidationError(ValidationFailure[] validationFailures)
    {
        Error[] errors = validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray();
        return new(errors);
    }
}