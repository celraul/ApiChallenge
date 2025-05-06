using Cel.GameOfLife.Application.UseCases.MessageUseCases.Commands.GenerateNextState;
using FluentValidation.Results;

namespace Cel.GameOfLife.ApplicationUnitTest.UseCases.Commands;

public class GenerateNextStateCommandValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ShouldBeInvalid_WhenCommandIsInvalid()
    {
        // Arrange
        var validator = new GenerateNextStateCommandValidator();

        // Act 
        ValidationResult result = await validator.ValidateAsync(new GenerateNextStateCommand(string.Empty, 0));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAsync_ShouldBevalid_WhenCommandIsValid()
    {
        // Arrange
        var validator = new GenerateNextStateCommandValidator();

        // Act 
        ValidationResult result = await validator.ValidateAsync(new GenerateNextStateCommand("Id", 1));

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
