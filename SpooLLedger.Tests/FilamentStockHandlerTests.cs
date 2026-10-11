using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Commands;
using SpooLLedger.Application.FilamentStock.Queries.GetFilamentStockById;
using SpooLLedger.Application.FilamentStock.Queries.GetFilamentStocks;
using SpooLLedger.Domain;

namespace SpooLLedger.Tests;

public class FilamentStockHandlerTests
{
    private readonly Mock<IFilamentStockRepository> _repository = new();

    [Fact]
    public async Task Create_PersistsAndReturnsDto()
    {
        // Arrange
        FilamentStock? saved = null;
        _repository
            .Setup(r => r.AddAsync(It.IsAny<FilamentStock>(), It.IsAny<CancellationToken>()))
            .Callback<FilamentStock, CancellationToken>((stock, _) =>
            {
                stock.Id = 7;
                saved = stock;
            })
            .Returns(Task.CompletedTask);

        var handler = new CreateFilamentStockHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new CreateFilamentStockCommand("Prusa", "PLA", "Black", 2), CancellationToken.None);

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(7, result.Id);
        Assert.Equal("Prusa", result.Producer);
        Assert.Equal("PLA", result.Type);
        Assert.Equal("Black", result.Color);
        Assert.Equal(2, result.Quantity);
    }

    [Fact]
    public async Task GetById_ReturnsNullWhenMissing()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(4, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FilamentStock?)null);

        var handler = new GetFilamentStockByIdHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new GetFilamentStockByIdQuery(4), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ReturnsNullWhenMissing()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FilamentStock?)null);

        var handler = new UpdateFilamentStockHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new UpdateFilamentStockCommand(9, "Prusa", "PLA", "Red", 1), CancellationToken.None);

        // Assert
        Assert.Null(result);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<FilamentStock>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsFalseWhenMissing()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FilamentStock?)null);

        var handler = new DeleteFilamentStockHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteFilamentStockCommand(3), CancellationToken.None);

        // Assert
        Assert.False(result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<FilamentStock>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task List_MapsFieldsWithoutAutoMapper()
    {
        // Arrange
        _repository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FilamentStock>
            {
                new() { Id = 1, Producer = "Bambu", Type = "PETG", Color = "Blue", Quantity = 4 }
            });

        var handler = new GetFilamentStocksHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new GetFilamentStocksQuery(), CancellationToken.None);

        // Assert
        var stock = Assert.Single(result);
        Assert.Equal(1, stock.Id);
        Assert.Equal("Bambu", stock.Producer);
        Assert.Equal("PETG", stock.Type);
        Assert.Equal("Blue", stock.Color);
        Assert.Equal(4, stock.Quantity);
    }
}
