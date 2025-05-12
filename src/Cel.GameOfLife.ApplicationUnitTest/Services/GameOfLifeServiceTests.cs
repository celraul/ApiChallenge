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
        bool[][] initial =
         {
           [false, false, false],
           [true,  true,  true],
           [false, false, false]
        };

        bool[][] expected =
        {
            [ false, true, false ],
            [ false, true, false ],
            [ false, true, false ]
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        bool[][] result = await _gameOfLifeService.NextState(initial);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task NextState_ShouldReturnNextStateForBigOne()
    {
        // Arrange
        int size = 1000;
        bool[][] initial = new bool[size][];
        var rand = new Random();

        for (int i = 0; i < size; i++)
        {
            bool[] col = new bool[size];
            for (int j = 0; j < size; j++)
                col[j] = (rand.Next(2) == 1);

            initial[i] = col;
        }

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        bool[][] result = await _gameOfLifeService.NextState(initial);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FinalState_ShouldReturnTheFinalState()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        bool[][] initial =
        {
            [ true, false, false ],
            [ false, true, false ],
            [ false, false, true ]
        };
        bool[][] expectedFinal =
        {
            [false, false, false ],
            [false, false, false ],
            [false, false, false ]
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        bool[][] result = await _gameOfLifeService.FinalState(initial);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedFinal);
    }

    [Fact]
    public void FinalState_ShouldThrowEx_whenReachTheMaxNumberOfAttmpets()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        bool[][] initial =
        {
           [ false, false, false],
           [ true,  true,  true ],
           [ false, false, false]
        };

        Board board = new()
        {
            Id = "boardId",
            Name = "Name",
            Field = initial,
            CurrentState = initial
        };

        // Act
        Func<Task<bool[][]>> act = async () => await _gameOfLifeService.FinalState(initial);

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
        bool[][] initial = // glinder
        {
            [true, true, true ],
            [true, false, false ],
            [false, true, false ]
        };

        // Act
        int result = await GameOfLifeService.CountLiveNeighbors(initial, row, col);

        // Assert
        result.Should().Be(expectedCount);
    }
}
