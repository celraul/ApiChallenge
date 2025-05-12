using Cel.GameOfLife.Application.Consts;
using Cel.GameOfLife.Application.Extensions;
using Cel.GameOfLife.Application.Interfaces;

namespace Cel.GameOfLife.Application.Services;

/// <summary>
/// rules:
/// 
/// Underpopulation:
/// A living cell with fewer than 2 live neighbors dies.
/// 
/// Survival:
/// A living cell with 2 or 3 live neighbors stays alive.
/// 
/// Overpopulation:
/// A living cell with more than 3 live neighbors dies.
/// 
/// Reproduction:
/// A dead cell with exactly 3 live neighbors becomes alive.
/// </summary>
public class GameOfLifeService : IGameOfLifeService
{
    private readonly int[] NumbersOfNeighborsToKeepAlive = [2, 3];
    private readonly int NumbersOfNeighborsToWakeUp = 3;

    public async Task<List<List<bool>>> NextState(List<List<bool>> currentState, int rounds = 1)
    {
        for (int i = 0; i < rounds; i++)
            currentState = await GetNextStateAsync(currentState);

        return currentState;
    }

    /// <summary>
    /// It returns the final state of the board.
    /// </summary>
    /// <param name="currentState"></param>
    /// <returns></returns>
    public async Task<List<List<bool>>> FinalState(List<List<bool>> currentState)
    {
        int count = 0;
        for (int i = 0; i < GameOfLifeConsts.MaxRounds; i++)
        {
            if (currentState.IsAllDead())
                break;

            List<List<bool>> nextState = await GetNextStateAsync(currentState);
            if (currentState.AreEqual(nextState))
                break;

            currentState = nextState;
            count++;
        }

        if (!currentState.IsAllDead())
            throw new Exception($"board doesn't go to conclusion after {count} attempts.");

        return currentState;
    }

    /// <summary>
    /// It returns the next state of the board. interate over rows and columns and apply the rules of the Game of Life.
    /// </summary>
    /// <param name="currentState"></param>
    /// <returns></returns>
    private async Task<List<List<bool>>> GetNextStateAsync(List<List<bool>> currentState)
    {
        int rows = currentState.Count;
        int cols = currentState[0].Count;

        var rowTasks = new List<Task<List<bool>>>(rows);

        for (int row = 0; row < rows; row++)
        {
            int currentRow = row;
            rowTasks.Add(Task.Run(async () =>
            {
                var newRow = new List<bool>(cols);
                for (int col = 0; col < cols; col++)
                {
                    int liveNeighbors = await CountLiveNeighbors(currentState, currentRow, col);

                    // Apply the rules of the Game of Life
                    bool isAlive = currentState[currentRow][col];
                    bool nextState = isAlive ? NumbersOfNeighborsToKeepAlive.Contains(liveNeighbors) :
                                                liveNeighbors == NumbersOfNeighborsToWakeUp;
                    newRow.Add(nextState);
                }
                return newRow;
            }));
        }

        var next = await Task.WhenAll(rowTasks);
        return next.ToList();
    }

    /// <summary>
    /// It returns the number of live neighbors.
    /// </summary>
    /// <param name="board">To get size of board.</param>
    /// <param name="row">Row of current item (position).</param>
    /// <param name="col">Row of current item (position).</param>
    public static Task<int> CountLiveNeighbors(List<List<bool>> board, int row, int col)
    {
        int countOfLiveNeighbors = 0;

        int rows = board.Count;
        int cols = board[0].Count;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (i == row && j == col)
                    continue;

                if (i >= 0 && j >= 0 && i < rows && j < cols && board[i][j])
                    countOfLiveNeighbors++;
            }
        }

        return Task.FromResult(countOfLiveNeighbors);
    }
}
