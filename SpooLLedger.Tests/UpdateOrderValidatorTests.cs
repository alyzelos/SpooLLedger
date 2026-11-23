using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class UpdateOrderValidatorTests
{
    [Fact]
    public async Task StatusChange_KeepsFilamentTypeAlreadyStoredOnTheOrder()
    {
        // Arrange
        var filamentTypes = new Mock<IFilamentTypeRepository>();
        filamentTypes
            .Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var filamentColors = new Mock<IFilamentColorRepository>();
        filamentColors
            .Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var pricings = new Mock<IPricingRepository>();
        pricings
            .Setup(r => r.NameExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var orders = new Mock<IOrderRepository>();
        orders
            .Setup(r => r.GetByOrderNumber(8, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order
            {
                OrderNumber = 8,
                Items = [new OrderItem { FilamentType = "PLA", Color = "Azure Blue", Quantity = 1 }],
            });

        var validator = new UpdateOrderValidator(
            filamentTypes.Object,
            filamentColors.Object,
            pricings.Object,
            orders.Object);

        var command = new UpdateOrderCommand
        {
            OrderNumber = 8,
            OrderDate = DateTime.UtcNow,
            Status = "Printing",
            PricingName = "Standard",
            CustomerId = 1,
            Items =
            [
                new OrderItemRequest
                {
                    ItemId = 1,
                    FilamentType = "PLA",
                    Color = "Azure Blue",
                    Quantity = 1,
                },
            ],
        };

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.DoesNotContain(result.Errors, error => error.ErrorMessage == "Choose a filament type that exists.");
    }
}
