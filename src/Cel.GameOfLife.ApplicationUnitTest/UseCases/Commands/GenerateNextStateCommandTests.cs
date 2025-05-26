using Cel.Core.Mediator.Models;
using Cel.GameOfLife.Application.Interfaces;
using Cel.GameOfLife.Application.Models;
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
        bool[][] initial =
        {
            [ false, false, false ],
            [ true,  true,  true  ],
            [ false, false, false ]
        };
        bool[][] expected =
        {
            [false, true,  false ],
            [false, true,  false ],
            [false, true,  false ]
        };

        string id = "boardId";
        var command = new GenerateNextStateCommand(id, 1);
        _repository.Setup(x => x.GetById(id)).ReturnsAsync(new Board { Id = id, Name = "Name", Field = initial, CurrentState = initial, Generation = 1 });

        _gameOfLifeService.Setup(x => x.NextState(initial, 1)).ReturnsAsync(expected);

        // Act
        Result<BoardModel> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.CurrentState.Should().BeEquivalentTo(expected);
        result.Value.Generation.Should().Be(2);

        _gameOfLifeService.Verify(x => x.NextState(initial, 1), Times.Once);
    }
}
