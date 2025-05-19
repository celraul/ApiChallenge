using Cel.Core.Mediator.Interfaces;
using Cel.Core.Mediator.Models;
using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Queries.GetNextState;

public record GetFinalStateQuery(string Id) : IQuery<Result<bool[][]>>;

public class GetFinalStateQueryHandler : IQueryHandler<GetFinalStateQuery, Result<bool[][]>>
{
    private readonly IRepository<Board> _repository;
    private readonly IGameOfLifeService _service;

    public GetFinalStateQueryHandler(IRepository<Board> repository, IGameOfLifeService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Result<bool[][]>> Handle(GetFinalStateQuery request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id);
        if (board is null)
            return Result.Failure<bool[][]>(Error.NotFound("Board not found."));

        return await _service.FinalState(board.CurrentState);
    }
}
