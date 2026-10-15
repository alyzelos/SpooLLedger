using System;
using System.ComponentModel.Design;
using MediatR;
using SpooLLedger.Application.CQRS.Customer.Responses;

namespace SpooLLedger.Application.CQRS.Customer.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerQueryResponse>
{
    public GetCustomerByIdQuery(int id)
    {
        Id=id;
    }
    public int Id { get; set; }
}
