using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Queries;

public class GetCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<GetCustomerQuery, IReadOnlyList<CustomerQueryResponse>>
{
    public async Task<IReadOnlyList<CustomerQueryResponse>> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAllCustomers(cancellationToken);
        return mapper.Map<IReadOnlyList<CustomerQueryResponse>>(customer);
    }
}