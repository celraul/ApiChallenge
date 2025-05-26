using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.ResetBoard;

public record ResetBoardCommand(string Id) : ICommand<Result> { }

public class ResetBoardCommandHandler(IRepository<Board> repository) : ICommandHandler<ResetBoardCommand, Result>
{
    private readonly IRepository<Board> _repository = repository;

    public async Task<Result> Handle(ResetBoardCommand request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id);
        if (board is null)
            return Result.Failure(Error.NotFound("Board not found."));

        board.CurrentState = board.Field;
        board.Generation = default;

        await _repository.UpdateAsync(board);

        return Result.Success(); // No need to return anything, just reset the board
    }
}