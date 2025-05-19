using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.Models;
using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;

public record GenerateNextStateCommand(string Id, int Count = 1) : ICommand<Result<BoardModel>> { }

public class GenerateNextStateCommandHandler(IRepository<Board> repository, IGameOfLifeService service)
    : ICommandHandler<GenerateNextStateCommand, Result<BoardModel>>
{
    private readonly IRepository<Board> _repository = repository;
    private readonly IGameOfLifeService _service = service;

    public async Task<Result<BoardModel>> Handle(GenerateNextStateCommand request, CancellationToken cancellationToken)
    {
        Board? board = await _repository.GetById(request.Id);

        if (board is null)
            return Result.Failure<BoardModel>(Error.NotFound("Board not found."));

        board.CurrentState = await _service.NextState(board.CurrentState, request.Count);
        board.Generation += request.Count;

        await _repository.UpdateAsync(board);
        BoardModel result = board; // implicit conversion

        return result;
    }
}
