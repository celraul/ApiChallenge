﻿using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Domain.Entities;
using Cel.GameOfLife.Domain.Exceptions;
using MediatR;

namespace Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.ResetBoard;

public record ResetBoardCommand(string Id) : IRequest { }

public class ResetBoardCommandHandler(IRepository<Board> repository)
    : IRequestHandler<ResetBoardCommand>
{
    private readonly IRepository<Board> _repository = repository;

    public async Task Handle(ResetBoardCommand request, CancellationToken cancellationToken)
    {
        Board board = await _repository.GetById(request.Id) ??
            throw new NotFoundException("Board not found.");

        board.CurrentState = board.Field;
        board.Generation = default;

        await _repository.UpdateAsync(board);
    }
}