using Cel.GameOfLife.Application.Consts;
using Cel.GameOfLife.Application.Services;
using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.ApplicationUnitTest.Services;

public class GameOfLifeServiceTests
{
    private readonly GameOfLifeService _gameOfLifeService;

    public GameOfLifeServiceTests()
    {
        _gameOfLifeService = new GameOfLifeService();
    }

    [Fact]
    public void NextState_ShouldReturnNextState()
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
        var result = _gameOfLifeService.NextState(initial);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void FinalState_ShouldReturnTheFinalState()
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
        var result = _gameOfLifeService.FinalState(initial);

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
        Func<List<List<bool>>> act = () => _gameOfLifeService.FinalState(initial);

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage($"board doesn't go to conclusion after {GameOfLifeConsts.MaxRounds} attempts.");
    }

    [Theory]
    [InlineData(0, 0, 2)]
    [InlineData(0, 1, 3)]
    [InlineData(1, 1, 5)]
    [InlineData(2, 2, 1)]
    public void CountLiveNeighbors_ShouldReturnNumberOfLiveNeighbors(int row, int col, int expectedCount)
    {
        // Arrange
        var initial = new List<List<bool>>() // glinder
        {
            new() { true, true, true },
            new() { true,  false,  false },
            new() { false, true, false }
        };

        // Act
        int result = GameOfLifeService.CountLiveNeighbors(initial, row, col);

        // Assert
        result.Should().Be(expectedCount);
    }
}
