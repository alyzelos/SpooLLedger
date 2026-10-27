using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public class CreateCustomerHandler(ICustomerRepository repository)
    : IRequestHandler<CreateCustomerCommand, CustomerQueryResponse>
{
    public async Task<CustomerQueryResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = CustomerMapper.ToEntity(request);
        await repository.AddAsync(customer, cancellationToken);
        return CustomerMapper.ToResponse(customer);
    }
}
