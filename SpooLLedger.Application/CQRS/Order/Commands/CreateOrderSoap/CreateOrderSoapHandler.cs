using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;

public class CreateOrderSoapHandler(
    IOrderRepository orderRepository,
    IPricingRepository pricings,
    IItemPriceCalculator calculator,
    IMapper mapper) : IRequestHandler<CreateOrderSoapCommand, CreateOrderSoapResponse>
{
    public async Task<CreateOrderSoapResponse> Handle(CreateOrderSoapCommand command, CancellationToken cancellationToken)
    {
        var createOrder = mapper.Map<CreateOrderCommand>(command);
        var createdOrder = await orderRepository.CreateOrder(createOrder, cancellationToken);
        var response = mapper.Map<CreateOrderSoapResponse>(createdOrder);
        await OrderPrices.Apply([response], pricings, calculator, cancellationToken);
        return response;
    }
}
