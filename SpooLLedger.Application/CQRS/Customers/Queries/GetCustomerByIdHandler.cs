using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Queries;

public class GetCustomerByIdHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerQueryResponse>
{
    public async Task<CustomerQueryResponse> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var order =  await customerRepository.GetCustomerById(query.Id, cancellationToken);
        return mapper.Map<CustomerQueryResponse>(order);
    }
}
