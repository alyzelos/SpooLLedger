using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentTypes.Commands;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class FilamentTypeTests
{
    private readonly Mock<IFilamentTypeRepository> _repository = new();

    [Fact]
    public async Task Create_TrimsAndPersists()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<FilamentType>(), It.IsAny<CancellationToken>()))
            .Callback<FilamentType, CancellationToken>((type, _) => type.Id = 6)
            .Returns(Task.CompletedTask);

        var handler = new CreateFilamentTypeHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new CreateFilamentTypeCommand("  Nylon  "), CancellationToken.None);

        // Assert
        Assert.Equal(6, result.Id);
        Assert.Equal("Nylon", result.Name);
    }

    [Fact]
    public async Task Delete_RefusesTypeUsedByStock()
    {
        // Arrange
        _repository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new FilamentType { Id = 1, Name = "PLA" });
        _repository.Setup(r => r.IsUsedByStockAsync("PLA", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteFilamentTypeHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteFilamentTypeCommand(1), CancellationToken.None);

        // Assert
        Assert.Equal(DeleteFilamentTypeResult.UsedByStock, result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<FilamentType>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_PassesPreviousNameSoStockCanFollowTheRename()
    {
        // Arrange
        var type = new FilamentType { Id = 2, Name = "PETG" };
        _repository.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(type);

        var handler = new UpdateFilamentTypeHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new UpdateFilamentTypeCommand(2, "PET-G"), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("PET-G", result.Name);
        _repository.Verify(r => r.UpdateAsync(type, "PETG", It.IsAny<CancellationToken>()), Times.Once);
    }
}
