using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;

public class UpdateOrderHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{

    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await orderRepository.UpdateOrder(command, cancellationToken);
        return mapper.Map<UpdateOrderResponse>(result);
    }
}
