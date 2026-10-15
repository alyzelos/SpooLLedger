using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderHandler(
    IOrderRepository orderRepository,
    IMapper mapper) 
    : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var createdOrder =  await orderRepository.CreateOrder(command, cancellationToken);
        return mapper.Map<CreateOrderResponse>(createdOrder); 
    }
}
