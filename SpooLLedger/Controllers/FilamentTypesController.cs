using MediatR;
using Microsoft.AspNetCore.Mvc;
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
}
