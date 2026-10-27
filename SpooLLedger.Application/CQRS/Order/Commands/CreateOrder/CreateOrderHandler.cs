using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderHandler(
    IOrderRepository orderRepository,
    IPricingRepository pricings,
    IItemPriceCalculator calculator,
    IMapper mapper) 
    : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var createdOrder =  await orderRepository.CreateOrder(command, cancellationToken);
        var response = mapper.Map<CreateOrderResponse>(createdOrder);
        await OrderPrices.Apply([response], pricings, calculator, cancellationToken);
        return response;
    }
}
