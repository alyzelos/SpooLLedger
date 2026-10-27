using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;

public class UpdateOrderHandler(
    IOrderRepository orderRepository,
    IPricingRepository pricings,
    IItemPriceCalculator calculator,
    IMapper mapper) : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{

    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await orderRepository.UpdateOrder(command, cancellationToken);
        var response = mapper.Map<UpdateOrderResponse>(result);
        await OrderPrices.Apply([response], pricings, calculator, cancellationToken);
        return response;
    }
}
