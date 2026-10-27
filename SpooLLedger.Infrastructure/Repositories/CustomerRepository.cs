using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class CustomerRepository(DataContext dataContext) : ICustomerRepository
{
    public async Task<IReadOnlyList<Customer>> GetAllCustomers(CancellationToken cancellationToken)=> 
        await dataContext.Customers.AsNoTracking().ToListAsync(cancellationToken);
    
    public async Task<Customer> GetCustomerById(int id, CancellationToken cancellationToken)
    {
        var order = await dataContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c=>c.Id == id, cancellationToken);

        if (order is null)
        {
            throw new Exception ("customer not found");
        }
        return order;
    }

    public Task<Customer?> FindAsync(int id, CancellationToken cancellationToken) =>
        dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<bool> IsUsedInOrdersAsync(int id, CancellationToken cancellationToken) =>
        dataContext.Orders.AnyAsync(o => o.CustomerId == id, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        dataContext.Customers.Add(customer);
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        dataContext.Customers.Update(customer);
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
    {
        dataContext.Customers.Remove(customer);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
