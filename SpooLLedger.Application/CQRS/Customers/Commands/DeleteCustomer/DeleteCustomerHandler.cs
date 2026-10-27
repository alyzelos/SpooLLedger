using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public class DeleteCustomerHandler(ICustomerRepository repository)
    : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResult>
{
    public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.FindAsync(request.Id, cancellationToken);
        if (customer is null)
            return DeleteCustomerResult.NotFound;

        if (await repository.IsUsedInOrdersAsync(request.Id, cancellationToken))
            return DeleteCustomerResult.UsedInOrders;

        await repository.DeleteAsync(customer, cancellationToken);
        return DeleteCustomerResult.Deleted;
    }
}
