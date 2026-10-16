using System;

namespace SpooLLedger.Application.CQRS.Items.Responses;

public class GetItemResponse
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int Weight { get; set; }
}
