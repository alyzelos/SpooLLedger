using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.FilamentTypes.Commands;
using SpooLLedger.Application.FilamentTypes.Dtos;
using SpooLLedger.Application.FilamentTypes.Queries.GetFilamentTypes;

namespace SpooLLedger.Controllers;

[Route("api/filament-types")]
[ApiController]
public class FilamentTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FilamentTypeDto>>> GetFilamentTypes(CancellationToken cancellationToken)
    {
        var types = await mediator.Send(new GetFilamentTypesQuery(), cancellationToken);
        return Ok(types);
    }

    [HttpPost]
    public async Task<ActionResult<FilamentTypeDto>> CreateFilamentType(
        CreateFilamentTypeCommand command,
        CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFilamentTypes), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FilamentTypeDto>> UpdateFilamentType(
        int id,
        UpdateFilamentTypeRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(new UpdateFilamentTypeCommand(id, request.Name), cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFilamentType(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteFilamentTypeCommand(id), cancellationToken);
        return result switch
        {
            DeleteFilamentTypeResult.Deleted => NoContent(),
            DeleteFilamentTypeResult.UsedByStock => Problem(
                title: "Filament type is used by stock",
                detail: "Change or remove the stock rows that use this type before deleting it.",
                statusCode: StatusCodes.Status409Conflict),
            _ => NotFound()
        };
    }
}

public record UpdateFilamentTypeRequest(string Name);
