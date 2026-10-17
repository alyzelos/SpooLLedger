using System;
using MediatR;
using SpooLLedger.Application.CQRS.Customers.Responses;

namespace SpooLLedger.Application.CQRS.Customers.Queries;

public class GetCustomerQuery : IRequest<IReadOnlyList<CustomerQueryResponse>>
{

}
