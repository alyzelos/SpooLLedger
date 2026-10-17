using System;
using System.Net;

namespace SpooLLedger.Application.CQRS.Order.Responses;

public class CustomOrderCommandResponse
{
    public int OrderNumber { get; set; }
    public string? Status { get; set; }
    public CustorOrderCustomerResponse? CustomerName { get; set; }

}
