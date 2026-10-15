using System;

namespace SpooLLedger.Application.CQRS.Order.Responses;

public abstract class OrderResponseBase
{
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
