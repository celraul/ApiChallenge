using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.Models;
using Cel.GameOfLife.Domain.Entities;
using MediatR;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;

public record GenerateNextStateCommand(string Id, int Count = 1) : IRequest<BoardModel> { }

public class GenerateNextStateCommandHandler(IRepository<Board> repository, IGameOfLifeService service)
    : IRequestHandler<GenerateNextStateCommand, BoardModel>
{
    private readonly IRepository<Board> _repository = repository;
    private readonly IGameOfLifeService _service = service;

    public async Task<BoardModel> Handle(GenerateNextStateCommand request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id) ??
            throw new KeyNotFoundException("Board not found."); // It can be improve using result patterns

        board.CurrentState = await _service.NextState(board.CurrentState, request.Count);
        board.Generation += request.Count;

        await _repository.UpdateAsync(board);

        return board; // implicit conversion
    }
}
