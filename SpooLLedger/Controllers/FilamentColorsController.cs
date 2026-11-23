using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.FilamentColors.Commands;
using SpooLLedger.Application.FilamentColors.Dtos;
using SpooLLedger.Application.FilamentColors.Queries.GetFilamentColors;

namespace SpooLLedger.Controllers;

[Route("api/filament-colors")]
[ApiController]
public class FilamentColorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FilamentColorDto>>> GetFilamentColors(CancellationToken cancellationToken)
    {
        var colors = await mediator.Send(new GetFilamentColorsQuery(), cancellationToken);
        return Ok(colors);
    }

    [HttpPost]
    public async Task<ActionResult<FilamentColorDto>> CreateFilamentColor(
        CreateFilamentColorCommand command,
        CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFilamentColors), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FilamentColorDto>> UpdateFilamentColor(
        int id,
        UpdateFilamentColorRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(new UpdateFilamentColorCommand(id, request.Name), cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFilamentColor(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteFilamentColorCommand(id), cancellationToken);
        return result switch
        {
            DeleteFilamentColorResult.Deleted => NoContent(),
            DeleteFilamentColorResult.UsedByStock => Problem(
                title: "Filament color is used by stock",
                detail: "Change or remove the stock rows that use this color before deleting it.",
                statusCode: StatusCodes.Status409Conflict),
            _ => NotFound()
        };
    }
}

public record UpdateFilamentColorRequest(string Name);
