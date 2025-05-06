using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.Models;
using Cel.GameOfLife.Domain.Entities;
using MediatR;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.CreateBoard;

public record CreateBoardCommand(CreateBoardModel BoardModel) : IRequest<string> { }

public class CreateBoardCommandHandler(IRepository<Board> repository) : IRequestHandler<CreateBoardCommand, string>
{
    private readonly IRepository<Board> _repository = repository;

    public async Task<string> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        // Using validator to valid the command.
        Board board = new()
        {
            Name = request.BoardModel.Name,
            Field = request.BoardModel.BoardState,
            CurrentState = request.BoardModel.BoardState,
        };

        await _repository.InsertAsync(board);

        return board.Id;
    }
}