namespace Cel.GameOfLife.Application.Interfaces;

public interface IGameOfLifeService
{
    List<List<bool>> NextState(List<List<bool>> currentState, int rounds = 1);
    List<List<bool>> FinalState(List<List<bool>> currentState);
}
