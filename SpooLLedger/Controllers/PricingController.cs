using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.Pricings.Commands;
using SpooLLedger.Application.Pricings.Dtos;
using SpooLLedger.Application.Pricings.Queries.GetPricingById;
using SpooLLedger.Application.Pricings.Queries.GetPricings;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PricingController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PricingDto>>> GetPricings(CancellationToken cancellationToken)
    {
        var pricings = await mediator.Send(new GetPricingsQuery(), cancellationToken);
        return Ok(pricings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PricingDto>> GetPricingById(int id, CancellationToken cancellationToken)
    {
        var pricing = await mediator.Send(new GetPricingByIdQuery(id), cancellationToken);
        return pricing is null ? NotFound() : Ok(pricing);
    }

    [HttpPost]
    public async Task<ActionResult<PricingDto>> CreatePricing(
        CreatePricingCommand command,
        CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetPricingById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PricingDto>> UpdatePricing(
        int id,
        UpdatePricingRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(
            new UpdatePricingCommand(
                id,
                request.Name,
                request.ElectricityCostPerKwh,
                request.FilamentSpoolPrice,
                request.PrinterMaintenanceCostPerHour),
            cancellationToken);

        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePricing(int id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeletePricingCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

public record UpdatePricingRequest(
    string Name,
    decimal ElectricityCostPerKwh,
    decimal FilamentSpoolPrice,
    decimal PrinterMaintenanceCostPerHour);
