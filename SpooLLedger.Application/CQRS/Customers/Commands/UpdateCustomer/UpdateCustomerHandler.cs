using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public class UpdateCustomerHandler(ICustomerRepository repository)
    : IRequestHandler<UpdateCustomerCommand, CustomerQueryResponse?>
{
    public async Task<CustomerQueryResponse?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.FindAsync(request.Id, cancellationToken);
        if (customer is null)
            return null;

        CustomerMapper.Apply(request, customer);
        await repository.UpdateAsync(customer, cancellationToken);
        return CustomerMapper.ToResponse(customer);
    }
}
