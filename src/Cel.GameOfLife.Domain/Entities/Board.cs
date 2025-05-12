namespace Cel.GameOfLife.Domain.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool[][] Field { get; set; } = null!;
    public bool[][] CurrentState { get; set; } = null!;
    public int Generation { get; set; }
}
