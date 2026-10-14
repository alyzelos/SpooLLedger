using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IOrderRspository
{
    public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken);
}
