using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CustomCommand;

public class CustomOrderHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<CustomOrderCommandQuery, CustomOrderCommandResponse>
{
    public async Task<CustomOrderCommandResponse> Handle(CustomOrderCommandQuery command, CancellationToken cancellationToken)
    {
        var result =  await orderRepository.GetCustomOrder(command.Id, cancellationToken);
        return mapper.Map<CustomOrderCommandResponse>(result);
    }
}
