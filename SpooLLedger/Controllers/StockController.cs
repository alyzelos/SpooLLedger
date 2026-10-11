using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.FilamentStock.Commands;
using SpooLLedger.Application.FilamentStock.Dtos;
using SpooLLedger.Application.FilamentStock.Queries.GetFilamentStockById;
using SpooLLedger.Application.FilamentStock.Queries.GetFilamentStocks;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FilamentStockDto>>> GetFilament(CancellationToken cancellationToken)
    {
        var stocks = await mediator.Send(new GetFilamentStocksQuery(), cancellationToken);
        return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FilamentStockDto>> GetFilamentById(int id, CancellationToken cancellationToken)
    {
        var stock = await mediator.Send(new GetFilamentStockByIdQuery(id), cancellationToken);
        return stock is null ? NotFound() : Ok(stock);
    }

    [HttpPost]
    public async Task<ActionResult<FilamentStockDto>> CreateFilament(
        CreateFilamentStockCommand command,
        CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFilamentById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FilamentStockDto>> UpdateFilament(
        int id,
        UpdateFilamentStockRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(
            new UpdateFilamentStockCommand(id, request.Producer, request.Type, request.Color, request.Quantity),
            cancellationToken);

        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFilament(int id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteFilamentStockCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

public record UpdateFilamentStockRequest(string Producer, string Type, string Color, int Quantity);
