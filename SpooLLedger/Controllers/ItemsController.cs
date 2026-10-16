using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Items.Commands.Create;
using SpooLLedger.Application.CQRS.Items.Queries;
using SpooLLedger.Application.CQRS.Items.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetItemResponse>>> GetItems(CancellationToken cancellationToken)
    {
        var items = await mediator.Send(new GetItemsQuery(), cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<GetItemResponse> GetItemById(int id, CancellationToken cancellationToken)
    {
        var query = new GetItemByIdQuery(id);
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("create-item")]
    public async Task<CreateItemResponse> CreateItem([FromBody] CreateItemCommand command, CancellationToken cancellationToken)
    {
        var createdItem = await mediator.Send(command, cancellationToken);
        return createdItem;
    }
}
