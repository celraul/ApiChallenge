namespace Cel.GameOfLife.Application.Models;

public class BoardModel : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public bool[,]? boardState { get; set; }
}
