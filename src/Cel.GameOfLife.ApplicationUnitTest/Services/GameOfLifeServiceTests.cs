using Cel.GameOfLife.Application.Consts;
using Cel.GameOfLife.Application.Services;
using Cel.GameOfLife.Domain.Entities;
using System.Threading.Tasks;

namespace Cel.GameOfLife.ApplicationUnitTest.Services;

public class GameOfLifeServiceTests
{
    private readonly GameOfLifeService _gameOfLifeService;

    public GameOfLifeServiceTests()
    {
        _gameOfLifeService = new GameOfLifeService();
    }

    [Fact]
    public async Task NextState_ShouldReturnNextState()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        var initial = new List<List<bool>>()
         {
            new() { false, false, false },
            new() { true,  true,  true },
            new() { false, false, false }
         };
        var expected = new List<List<bool>>()
        {
            new() { false, true,  false },
            new() { false, true,  false },
            new() { false, true,  false }
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        List<List<bool>> result = await _gameOfLifeService.NextState(initial);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task NextState_ShouldReturnNextStateForBigOne()
    {
        // Arrange
        var initial = new List<List<bool>>();
        var rand = new Random();
        int size = 10000;

        for (int i = 0; i < size; i++)
        {
            var row = new List<bool>();
            for (int j = 0; j < size; j++)
                row.Add(rand.Next(2) == 1);

            initial.Add(row);
        }

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        List<List<bool>> result = await _gameOfLifeService.NextState(initial);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FinalState_ShouldReturnTheFinalState()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        var initial = new List<List<bool>>()
        {
            new() { true, false, false },
            new() { false,  true,  false },
            new() { false, false, true }
        };
        var expectedFinal = new List<List<bool>>()
        {
            new() { false, false,  false },
            new() { false, false,  false },
            new() { false, false,  false }
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        List<List<bool>> result = await _gameOfLifeService.FinalState(initial);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedFinal);
    }

    [Fact]
    public void FinalState_ShouldThrowEx_whenReachTheMaxNumberOfAttmpets()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        var initial = new List<List<bool>>
        {
           new() { false, false, false },
           new() { true,  true,  true },
           new() { false, false, false }
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        Func<Task<List<List<bool>>>> act = async () => await _gameOfLifeService.FinalState(initial);

        // Assert
        act.Should().ThrowAsync<Exception>()
           .WithMessage($"board doesn't go to conclusion after {GameOfLifeConsts.MaxRounds} attempts.");
    }

    [Theory]
    [InlineData(0, 0, 2)]
    [InlineData(0, 1, 3)]
    [InlineData(1, 1, 5)]
    [InlineData(2, 2, 1)]
    public async Task CountLiveNeighbors_ShouldReturnNumberOfLiveNeighbors(int row, int col, int expectedCount)
    {
        // Arrange
        var initial = new List<List<bool>>() // glinder
        {
            new() { true, true, true },
            new() { true,  false,  false },
            new() { false, true, false }
        };

        // Act
        int result = await GameOfLifeService.CountLiveNeighbors(initial, row, col);

        // Assert
        result.Should().Be(expectedCount);
    }
}
