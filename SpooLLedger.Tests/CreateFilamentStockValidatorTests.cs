using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Commands;

namespace SpooLLedger.Tests;

public class CreateFilamentStockValidatorTests
{
    private readonly CreateFilamentStockValidator _validator;

    public CreateFilamentStockValidatorTests()
    {
        var filamentTypes = new Mock<IFilamentTypeRepository>();
        filamentTypes
            .Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string name, CancellationToken _) => name == "PLA");
        _validator = new CreateFilamentStockValidator(filamentTypes.Object);
    }

    [Fact]
    public async Task EmptyProducer_Fails()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("", "PLA", "Black", 1);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFilamentStockCommand.Producer));
    }

    [Fact]
    public async Task NegativeQuantity_Fails()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("Prusa", "PLA", "Black", -1);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFilamentStockCommand.Quantity));
    }

    [Fact]
    public async Task UnknownType_Fails()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("Prusa", "Wood", "Black", 1);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFilamentStockCommand.Type));
    }

    [Fact]
    public async Task ValidCommand_Passes()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("Prusa", "PLA", "Black", 0);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }
}
