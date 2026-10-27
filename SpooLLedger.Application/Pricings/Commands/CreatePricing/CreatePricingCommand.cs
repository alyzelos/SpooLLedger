using MediatR;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Commands;

public record CreatePricingCommand(
    string Name,
    decimal ElectricityCostPerKwh,
    decimal FilamentSpoolPrice,
    decimal PrinterMaintenanceCostPerHour) : IRequest<PricingDto>;
