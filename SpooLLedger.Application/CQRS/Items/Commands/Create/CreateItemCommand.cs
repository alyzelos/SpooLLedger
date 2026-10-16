using System;
using MediatR;
using SpooLLedger.Application.CQRS.Items.Responses;

namespace SpooLLedger.Application.CQRS.Items.Commands.Create;

public class CreateItemCommand : IRequest<CreateItemResponse>
{
    public string? ProductName { get; set; }
    public int Weight { get; set; }
}
