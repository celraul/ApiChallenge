namespace Cel.GameOfLife.Application.Models;

public class CreateBoardModel
{
    public string Name { get; set; } = string.Empty;
    public bool[][] BoardState { get; set; } = [];
}
