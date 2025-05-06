using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;
using MediatR;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;

public record GenerateNextStateCommand(string Id, int Count = 1) : IRequest<List<List<bool>>> { }

public class GenerateNextStateCommandHandler(IRepository<Board> repository, IGameOfLifeService service)
    : IRequestHandler<GenerateNextStateCommand, List<List<bool>>>
{
    private readonly IRepository<Board> _repository = repository;
    private readonly IGameOfLifeService _service = service;

    public async Task<List<List<bool>>> Handle(GenerateNextStateCommand request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id) ??
            throw new KeyNotFoundException("Board not found."); // It can be improve using result patterns

        board.CurrentState = _service.NextState(board.CurrentState, request.Count);
        await _repository.UpdateAsync(board);

        return board.CurrentState;
    }
}
