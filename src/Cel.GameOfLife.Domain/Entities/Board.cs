namespace Cel.GameOfLife.Domain.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<List<bool>> Field { get; set; } = null!;
    public List<List<bool>> CurrentState { get; set; } = null!;
    public int Generation { get; set; }
}
