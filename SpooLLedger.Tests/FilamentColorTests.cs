using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentColors.Commands;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class FilamentColorTests
{
    private readonly Mock<IFilamentColorRepository> _repository = new();

    [Fact]
    public async Task Create_TrimsAndPersists()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<FilamentColor>(), It.IsAny<CancellationToken>()))
            .Callback<FilamentColor, CancellationToken>((color, _) => color.Id = 6)
            .Returns(Task.CompletedTask);

        var handler = new CreateFilamentColorHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new CreateFilamentColorCommand("  Forest Green  "), CancellationToken.None);

        // Assert
        Assert.Equal(6, result.Id);
        Assert.Equal("Forest Green", result.Name);
    }

    [Fact]
    public async Task Delete_RefusesColorUsedByStock()
    {
        // Arrange
        _repository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new FilamentColor { Id = 1, Name = "Azure Blue" });
        _repository.Setup(r => r.IsUsedByStockAsync("Azure Blue", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteFilamentColorHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteFilamentColorCommand(1), CancellationToken.None);

        // Assert
        Assert.Equal(DeleteFilamentColorResult.UsedByStock, result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<FilamentColor>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_PassesPreviousNameSoStockCanFollowTheRename()
    {
        // Arrange
        var color = new FilamentColor { Id = 2, Name = "Red" };
        _repository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(color);

        var handler = new UpdateFilamentColorHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new UpdateFilamentColorCommand(2, "Lipstick Red"), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Lipstick Red", result.Name);
        _repository.Verify(r => r.UpdateAsync(color, "Red", It.IsAny<CancellationToken>()), Times.Once);
    }
}
