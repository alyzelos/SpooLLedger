using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Items.Commands;
using SpooLLedger.Application.CQRS.Items.Dtos;
using SpooLLedger.Application.CQRS.Items.Queries.GetItemById;
using SpooLLedger.Application.CQRS.Items.Queries.GetItems;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetItems(CancellationToken cancellationToken)
    {
        var items = await mediator.Send(new GetItemsQuery(), cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetItemById(int id, CancellationToken cancellationToken)
    {
        var item = await mediator.Send(new GetItemByIdQuery(id), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [HttpPost("create-item")]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetItemById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ItemDto>> UpdateItem(
        int id,
        UpdateItemRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(
            new UpdateItemCommand(id, request.ProductName, request.Weight, request.PrintDurationMinutes, request.Details),
            cancellationToken);

        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItem(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteItemCommand(id), cancellationToken);
        return result switch
        {
            DeleteItemResult.Deleted => NoContent(),
            DeleteItemResult.UsedInOrders => Problem(
                title: "Item is used in orders",
                detail: "Remove this item from every order before deleting it.",
                statusCode: StatusCodes.Status409Conflict),
            _ => NotFound()
        };
    }
}

public record UpdateItemRequest(string ProductName, int Weight, int PrintDurationMinutes, string? Details);
