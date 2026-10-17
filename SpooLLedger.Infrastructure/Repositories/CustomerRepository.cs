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
}
