using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.DeleteOrder;
using SpooLLedger.Application.CQRS.Order.Commands.CustomCommand;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Application.CQRS.Order.Queries;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Soap;

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

    // [HttpGet("{orderNumber:int}")]
    // public async Task<ActionResult<OrderQueryResponse>> GetOrderByOrderNumber(int orderNumber, CancellationToken cancellationToken)
    // {
    //     var query = new GetOrderByOrderNumberQuery(orderNumber);
    //     return await mediator.Send(query, cancellationToken);
    // }

    [HttpGet("{orderNumber:int}")]
    public async Task<ActionResult<CustomOrderCommandResponse>> GetOrderByOrderNumber(int orderNumber, CancellationToken cancellationToken)
    {
        var query = new CustomOrderCommandQuery(orderNumber);
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("create-order")]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrder([FromBody]CreateOrderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPost("create-order-soap")]
    public async Task<ContentResult> CreateOrderSoap(CancellationToken cancellationToken)
    {
        var xml = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
        try
        {
            var command = CreateOrderSoapXml.Read(xml);
            var response = await mediator.Send(command, cancellationToken);
            return Xml(CreateOrderSoapXml.Write(response), StatusCodes.Status200OK);
        }
        catch (ValidationException exception)
        {
            var message = string.Join("; ", exception.Errors.Select(error => error.ErrorMessage));
            return Xml(CreateOrderSoapXml.WriteFault(message), StatusCodes.Status400BadRequest);
        }
        catch (Exception exception)
        {
            return Xml(CreateOrderSoapXml.WriteFault(exception.Message), StatusCodes.Status400BadRequest);
        }
    }

    [HttpDelete("{orderNumber:int}")]
    public async Task<IActionResult> DeleteOrder(int orderNumber, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteOrderCommand(orderNumber), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut("update")]
    public async Task<ActionResult<UpdateOrderResponse>> UpdateOrder([FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    private static ContentResult Xml(string content, int statusCode) =>
        new() { Content = content, ContentType = "text/xml", StatusCode = statusCode };
}
