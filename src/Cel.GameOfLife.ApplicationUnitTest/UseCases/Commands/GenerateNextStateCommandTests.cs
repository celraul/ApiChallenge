using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;
using Cel.GameOfLife.Domain.Entities;

namespace Cel.GameOfLife.ApplicationUnitTest.UseCases.Commands;

public class GenerateNextStateCommandTests
{
    private readonly Mock<IRepository<Board>> _repository;
    private readonly Mock<IGameOfLifeService> _gameOfLifeService;
    private readonly GenerateNextStateCommandHandler _handler;

    public GenerateNextStateCommandTests()
    {
        _repository = new Mock<IRepository<Board>>();
        _gameOfLifeService = new Mock<IGameOfLifeService>();
        _handler = new GenerateNextStateCommandHandler(_repository.Object, _gameOfLifeService.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateNextState()
    {
        // Arrange
        // Initial blinker (3x3 grid)
        var initial = new List<List<bool>>
        {
            new() { false, false, false },
            new() { true,  true,  true },
            new() { false, false, false }
        };
       var expected = new List<List<bool>>
        {
            new() { false, true,  false },
            new() { false, true,  false },
            new() { false, true,  false }
        };

        string id = "boardId";
        var command = new GenerateNextStateCommand(id, 1);
        _repository.Setup(x => x.GetById(id)).ReturnsAsync(new Board { Id = id, Name = "Name", Field = initial, CurrentState = initial });

        _gameOfLifeService.Setup(x => x.NextState(initial, 1)).Returns(expected);

        // Act
        List<List<bool>> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);

        _gameOfLifeService.Verify(x => x.NextState(initial, 1), Times.Once);
    }
}
