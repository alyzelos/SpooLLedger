using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;

public class CreateOrderSoapHandler(
    IOrderRepository orderRepository,
    IMapper mapper) : IRequestHandler<CreateOrderSoapCommand, CreateOrderSoapResponse>
{
    public async Task<CreateOrderSoapResponse> Handle(CreateOrderSoapCommand command, CancellationToken cancellationToken)
    {
        var createOrder = mapper.Map<CreateOrderCommand>(command);
        var createdOrder = await orderRepository.CreateOrder(createOrder, cancellationToken);
        return mapper.Map<CreateOrderSoapResponse>(createdOrder);
    }
}
