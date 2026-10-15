using System;
using SpooLLedger.Application.CQRS.Customer.Queries;
using SpooLLedger.Application.CQRS.Customer.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface ICustomerRepository
{
    public Task<IReadOnlyList<Customer>> GetAllCustomers(CancellationToken cancellationToken);
    public Task<Customer>GetCustomerById(int id, CancellationToken cancellationToken);
}
