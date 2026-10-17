using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CustomCommand;

public class CustomOrderCommandQuery : IRequest<CustomOrderCommandResponse>
{
    public CustomOrderCommandQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }

}
