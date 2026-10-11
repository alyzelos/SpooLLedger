using SpooLLedger.Application.FilamentStock.Commands;

namespace SpooLLedger.Tests;

public class CreateFilamentStockValidatorTests
{
    private readonly CreateFilamentStockValidator _validator = new();

    [Fact]
    public void EmptyProducer_Fails()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("", "PLA", "Black", 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFilamentStockCommand.Producer));
    }

    [Fact]
    public void NegativeQuantity_Fails()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("Prusa", "PLA", "Black", -1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateFilamentStockCommand.Quantity));
    }

    [Fact]
    public void ValidCommand_Passes()
    {
        // Arrange
        var command = new CreateFilamentStockCommand("Prusa", "PLA", "Black", 0);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}
