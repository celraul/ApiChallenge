namespace Cel.GameOfLife.Application.Interfaces;

public interface IGameOfLifeService
{
    Task<bool[][]> NextState(bool[][] currentState, int rounds = 1);
    Task<bool[][]> FinalState(bool[][] currentState);
}
