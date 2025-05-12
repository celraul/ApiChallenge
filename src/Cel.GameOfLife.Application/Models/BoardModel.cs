using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.Application.Models;

public class BoardModel : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public bool[][] Field { get; set; } = null!;
    public bool[][] CurrentState { get; set; } = null!;
    public int Generation { get; set; }

    public static implicit operator BoardModel(Board board)
        => new BoardModel
        {
            Id = board.Id,
            Name = board.Name,
            CurrentState = board.CurrentState,
            Field = board.Field,
            Generation = board.Generation
        };
}
