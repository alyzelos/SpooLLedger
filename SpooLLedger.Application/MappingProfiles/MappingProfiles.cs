using AutoMapper;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Order, OrderQueryResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));
        CreateMap<Order, CreateOrderResponse>();
            
    }
}
