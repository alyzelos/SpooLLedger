using AutoMapper;
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
    }
}
