namespace Cel.GameOfLife.Application.Interfaces;

public interface IGameOfLifeService
{
    Task<List<List<bool>>> NextState(List<List<bool>> currentState, int rounds = 1);
    Task<List<List<bool>>> FinalState(List<List<bool>> currentState);
}
