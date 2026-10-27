using SpooLLedger.Application.CQRS.Customers.Commands;
using SpooLLedger.Application.CQRS.Customers.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.CQRS.Customers;

public static class CustomerMapper
{
    public static CustomerQueryResponse ToResponse(Customer customer) => new()
    {
        Id = customer.Id,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        FullName = customer.FullName,
        Email = customer.Email,
        PhoneNumber = customer.PhoneNumber,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt
    };

    public static Customer ToEntity(CreateCustomerCommand command) => new()
    {
        FirstName = command.FirstName,
        LastName = command.LastName,
        Email = command.Email,
        PhoneNumber = command.PhoneNumber,
        IsActive = command.IsActive
    };

    public static void Apply(UpdateCustomerCommand command, Customer customer)
    {
        customer.FirstName = command.FirstName;
        customer.LastName = command.LastName;
        customer.Email = command.Email;
        customer.PhoneNumber = command.PhoneNumber;
        customer.IsActive = command.IsActive;
    }
}
