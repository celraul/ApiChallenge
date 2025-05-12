using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;
using Cel.GameOfLife.Domain.Exceptions;
using MediatR;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Queries.GetNextState;

public record GetFinalStateQuery(string Id) : IRequest<bool[][]>;

public class GetFinalStateQueryHandler : IRequestHandler<GetFinalStateQuery, bool[][]>
{
    private readonly IRepository<Board> _repository;
    private readonly IGameOfLifeService _service;

    public GetFinalStateQueryHandler(IRepository<Board> repository, IGameOfLifeService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<bool[][]> Handle(GetFinalStateQuery request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id) ??
            throw new NotFoundException("Board not found.");

        return await _service.FinalState(board.CurrentState);
    }
}
