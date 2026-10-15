using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpooLLedger.Application.CQRS.Customer.Queries;
using SpooLLedger.Application.CQRS.Customer.Responses;

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

}
