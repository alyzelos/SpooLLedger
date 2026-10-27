using AutoMapper;
using SpooLLedger.Application.CQRS.Customers.Responses;
using SpooLLedger.Application.CQRS.Items.Responses;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OrderItem, OrderLineResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Item.ProductName))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Item.Weight))
            .ForMember(dest => dest.PricePerUnit, opt => opt.MapFrom(src => src.Item.PricePerUnit));

        CreateMap<Order, OrderQueryResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));
        CreateMap<Order, CreateOrderResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));
        CreateMap<Order, UpdateOrderResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));
        CreateMap<CreateOrderSoapCommand, CreateOrderCommand>();
        CreateMap<Order, CreateOrderSoapResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));

        //custome query for command , mapping custom order response to map 
        //and !!!! important , mapping response of customer , to customOrderCustomerResponse;
        CreateMap<Order, CustomOrderCommandResponse>()
            .ForMember(dest=> dest.CustomerName, opt => opt.MapFrom(o => o.Customer));
        CreateMap<Customer, CustorOrderCustomerResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src=> src.FullName));
        //Customer mappers
        CreateMap<Customer, CustomerQueryResponse>();
        CreateMap<Item, GetItemResponse>();
        CreateMap<Item, CreateItemResponse>();
            
            
    }
}

