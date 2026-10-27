using System;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface ICustomerRepository
{
    public Task<IReadOnlyList<Customer>> GetAllCustomers(CancellationToken cancellationToken);
    public Task<Customer>GetCustomerById(int id, CancellationToken cancellationToken);
    Task<Customer?> FindAsync(int id, CancellationToken cancellationToken);
    Task<bool> IsUsedInOrdersAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    Task DeleteAsync(Customer customer, CancellationToken cancellationToken);
}
