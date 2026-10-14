using AutoMapper;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Order, OrderQueryResponse>();
    }
}
