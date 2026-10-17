using System;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface ICustomerRepository
{
    public Task<IReadOnlyList<Customer>> GetAllCustomers(CancellationToken cancellationToken);
    public Task<Customer>GetCustomerById(int id, CancellationToken cancellationToken);
}
