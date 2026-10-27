using Moq;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings;
using SpooLLedger.Application.Pricings.Commands;
using SpooLLedger.Application.Pricings.Queries.GetPricings;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Tests;

public class PricingTests
{
    private readonly Mock<IPricingRepository> _repository = new();

    [Fact]
    public async Task Create_PersistsAndReturnsPricePerGram()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<Pricing>(), It.IsAny<CancellationToken>()))
            .Callback<Pricing, CancellationToken>((pricing, _) => pricing.Id = 5)
            .Returns(Task.CompletedTask);

        var handler = new CreatePricingHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new CreatePricingCommand("Default", 0.25m, 25m, 0.5m), CancellationToken.None);

        // Assert
        Assert.Equal(5, result.Id);
        Assert.Equal("Default", result.Name);
        Assert.Equal(25m, result.FilamentSpoolPrice);
        Assert.Equal(0.025m, result.FilamentCostPerGram);
    }

    [Fact]
    public async Task Update_ReturnsNullWhenMissing()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pricing?)null);

        var handler = new UpdatePricingHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new UpdatePricingCommand(9, "Default", 0.25m, 25m, 0.5m), CancellationToken.None);

        // Assert
        Assert.Null(result);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<Pricing>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsFalseWhenMissing()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pricing?)null);

        var handler = new DeletePricingHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new DeletePricingCommand(3), CancellationToken.None);

        // Assert
        Assert.False(result);
        _repository.Verify(r => r.DeleteAsync(It.IsAny<Pricing>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task List_MapsFields()
    {
        // Arrange
        _repository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Pricing>
            {
                new() { Id = 1, Name = "Default", ElectricityCostPerKwh = 0.3m, FilamentSpoolPrice = 20m, PrinterMaintenanceCostPerHour = 0.1m }
            });

        var handler = new GetPricingsHandler(_repository.Object);

        // Act
        var result = await handler.Handle(new GetPricingsQuery(), CancellationToken.None);

        // Assert
        var pricing = Assert.Single(result);
        Assert.Equal(0.3m, pricing.ElectricityCostPerKwh);
        Assert.Equal(0.02m, pricing.FilamentCostPerGram);
        Assert.Equal(0.1m, pricing.PrinterMaintenanceCostPerHour);
    }

    [Fact]
    public async Task CreateValidator_RejectsNegativeCostsAndDuplicateName()
    {
        // Arrange
        _repository
            .Setup(r => r.NameExistsAsync("Default", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreatePricingValidator(_repository.Object);

        // Act
        var result = await validator.ValidateAsync(new CreatePricingCommand("Default", -1m, -1m, -1m));

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreatePricingCommand.Name));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreatePricingCommand.ElectricityCostPerKwh));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreatePricingCommand.FilamentSpoolPrice));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreatePricingCommand.PrinterMaintenanceCostPerHour));
    }

    [Fact]
    public void Calculate_Uses120WattsForTheFirstTenMinutesAnd80After()
    {
        // Arrange
        var item = new Item { Weight = 1000, PrintDurationMinutes = 70 };
        var pricing = new Pricing
        {
            ElectricityCostPerKwh = 1.5m,
            FilamentSpoolPrice = 30m,
            PrinterMaintenanceCostPerHour = 0.5m
        };

        // Act
        var result = new ItemPriceCalculator().Calculate(item, pricing);

        // Assert
        // 10 min at 120 W is 0.02 kWh, 60 min at 80 W is 0.08 kWh.
        Assert.Equal(0.10m, result.EnergyKwh);
        Assert.Equal(0.15m, result.Electricity);
        Assert.Equal(30m, result.Filament);
        Assert.Equal(70m / 60m * 0.5m, result.Maintenance);
    }

    [Fact]
    public void Calculate_ShortPrintStaysAt120Watts()
    {
        // Arrange
        var item = new Item { Weight = 0, PrintDurationMinutes = 5 };
        var pricing = new Pricing { ElectricityCostPerKwh = 1m, FilamentSpoolPrice = 0m };

        // Act
        var result = new ItemPriceCalculator().Calculate(item, pricing);

        // Assert
        Assert.Equal(5m / 60m * 120m / 1000m, result.EnergyKwh);
        Assert.Equal(0m, result.Maintenance);
    }

    [Fact]
    public async Task UpdateValidator_AllowsKeepingOwnName()
    {
        // Arrange
        _repository
            .Setup(r => r.NameExistsAsync("Default", 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdatePricingValidator(_repository.Object);

        // Act
        var result = await validator.ValidateAsync(new UpdatePricingCommand(2, "Default", 0.25m, 25m, 0.5m));

        // Assert
        Assert.True(result.IsValid);
    }
}
