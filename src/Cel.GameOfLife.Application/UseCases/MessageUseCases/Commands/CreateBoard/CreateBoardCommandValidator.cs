using FluentValidation;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.CreateBoard;

public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(item => item.BoardModel.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");

        RuleFor(item => item.BoardModel.BoardState)
            .NotNull()
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
    }
}
