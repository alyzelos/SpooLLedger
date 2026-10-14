using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class OrderRepository(DataContext dataContext) : IOrderRspository
{
    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken) => 
        await dataContext.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);   
}
