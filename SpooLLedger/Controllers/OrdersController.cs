using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Application.CQRS.Order.Queries;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderQueryResponse>>> GetOrders(CancellationToken cancellationToken)
    {
        var orders = await mediator.Send(new GetOrdersQuery(), cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{orderNumber:int}")]
    public async Task<ActionResult<OrderQueryResponse>> GetOrderByOrderNumber(int orderNumber, CancellationToken cancellationToken)
    {
        var query = new GetOrderByOrderNumberQuery(orderNumber);
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("create-order")]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrder([FromBody]CreateOrderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UpdateOrderResponse>> UpdateOrder([FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
}
