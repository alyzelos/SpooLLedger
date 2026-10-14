using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Order.Queries;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderQueryResponse>>> GetOrders(CancellationToken cancellationToken)
    {
        var orders = await mediator.Send(new GetOrdersQuery(), cancellationToken);
        return Ok(orders);
    }

}
