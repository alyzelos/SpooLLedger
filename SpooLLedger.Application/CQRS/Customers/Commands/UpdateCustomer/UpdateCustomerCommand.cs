using MediatR;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public record UpdateCustomerCommand(int Id, string FirstName, string LastName, string Email, string PhoneNumber, bool IsActive)
    : IRequest<CustomerQueryResponse?>;
