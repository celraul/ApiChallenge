using FluentValidation;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;

public class GenerateNextStateCommandValidator : AbstractValidator<GenerateNextStateCommand>
{
    public GenerateNextStateCommandValidator()
    {
        RuleFor(item => item.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");

        RuleFor(item => item.Count)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .WithMessage("{PropertyName} must to be greater or equal than 1.");
    }
}
