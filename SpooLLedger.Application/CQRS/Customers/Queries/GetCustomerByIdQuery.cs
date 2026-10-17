using System;
using System.ComponentModel.Design;
using MediatR;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerQueryResponse>
{
    public GetCustomerByIdQuery(int id)
    {
        Id=id;
    }
    public int Id { get; set; }
}
