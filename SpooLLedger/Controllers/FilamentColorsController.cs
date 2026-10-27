using MediatR;
using Microsoft.AspNetCore.Mvc;
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
}
