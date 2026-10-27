using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Customers.Commands;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class CustomerTests
{
    private readonly Mock<ICustomerRepository> _repository = new();

    [Fact]
    public async Task Create_PersistsAndReturnsFullName()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Callback<Customer, CancellationToken>((customer, _) => customer.Id = 8)
            .Returns(Task.CompletedTask);

        var handler = new CreateCustomerHandler(_repository.Object);

        // Act
        var result = await handler.Handle(
            new CreateCustomerCommand("Ada", "Lovelace", "ada@example.com", "+40 700 000 000", true),
            CancellationToken.None);

        // Assert
        Assert.Equal(8, result.Id);
        Assert.Equal("Ada Lovelace", result.FullName);
    }

    [Fact]
    public async Task Delete_RefusesCustomerUsedInOrders()
    {
        // Arrange
        _repository.Setup(r => r.FindAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Id = 1 });
        _repository.Setup(r => r.IsUsedInOrdersAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteCustomerHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeleteCustomerCommand(1), CancellationToken.None);

        // Assert
        Assert.Equal(DeleteCustomerResult.UsedInOrders, result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateValidator_RejectsInvalidEmail()
    {
        // Arrange
        var validator = new CreateCustomerValidator();

        // Act
        var result = await validator.ValidateAsync(new CreateCustomerCommand("Ada", "Lovelace", "not-an-email", "1", true));

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateCustomerCommand.Email));
    }
}
