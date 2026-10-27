using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Commands;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class ItemTests
{
    private readonly Mock<IItemsRepository> _repository = new();

    [Fact]
    public async Task Create_SavesWithoutPrice()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Callback<Item, CancellationToken>((item, _) => item.Id = 6)
            .Returns(Task.CompletedTask);

        var handler = new CreateItemHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new CreateItemCommand("Vase", 120, 300, "Spiral mode"), CancellationToken.None);

        // Assert
        Assert.Equal(6, result.Id);
        Assert.Equal(300, result.PrintDurationMinutes);
        Assert.Equal("Spiral mode", result.Details);
        Assert.Null(result.PricePerUnit);
    }

    [Fact]
    public async Task Update_KeepsExistingPrice()
    {
        // Arrange
        var item = new Item { Id = 2, ProductName = "Statue", Weight = 35, PrintDurationMinutes = 180, PricePerUnit = 27m };
        _repository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var handler = new UpdateItemHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new UpdateItemCommand(2, "Statue XL", 50, 240, null), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Statue XL", result.ProductName);
        Assert.Equal(240, result.PrintDurationMinutes);
        Assert.Equal(27m, result.PricePerUnit);
    }

    [Fact]
    public async Task Delete_RefusesItemUsedInOrders()
    {
        // Arrange
        _repository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Item { Id = 1 });
        _repository.Setup(r => r.IsUsedInOrdersAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteItemHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteItemCommand(1), CancellationToken.None);

        // Assert
        Assert.Equal(DeleteItemResult.UsedInOrders, result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<Item>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsNotFoundWhenMissing()
    {
        // Arrange
        _repository.Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync((Item?)null);

        var handler = new DeleteItemHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteItemCommand(9), CancellationToken.None);

        // Assert
        Assert.Equal(DeleteItemResult.NotFound, result);
    }

    [Fact]
    public async Task CreateValidator_RejectsZeroDurationAndWeight()
    {
        // Arrange
        var validator = new CreateItemValidator();

        // Act
        var result = await validator.ValidateAsync(new CreateItemCommand("Vase", 0, 0, null));

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItemCommand.Weight));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateItemCommand.PrintDurationMinutes));
    }
}
