using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Customers.Commands;
using SpooLLedger.Application.CQRS.Customers.Queries;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerQueryResponse>>> GetCustomers(CancellationToken cancellationToken)
    {
        var customers =  await mediator.Send(new GetCustomerQuery(), cancellationToken); 
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerQueryResponse>> GetCustomerById(int id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(id);
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerQueryResponse>> CreateCustomer(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var created = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerQueryResponse>> UpdateCustomer(
        int id,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(
            new UpdateCustomerCommand(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.IsActive),
            cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return result switch
        {
            DeleteCustomerResult.Deleted => NoContent(),
            DeleteCustomerResult.UsedInOrders => Problem(
                title: "Customer is used in orders",
                detail: "Remove this customer's orders before deleting them.",
                statusCode: StatusCodes.Status409Conflict),
            _ => NotFound()
        };
    }
}

public record UpdateCustomerRequest(string FirstName, string LastName, string Email, string PhoneNumber, bool IsActive);
