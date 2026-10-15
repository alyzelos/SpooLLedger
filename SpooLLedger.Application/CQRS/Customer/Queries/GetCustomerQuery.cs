using System;
using MediatR;
using SpooLLedger.Application.CQRS.Customer.Responses;

namespace SpooLLedger.Application.CQRS.Customer.Queries;

public class GetCustomerQuery : IRequest<IReadOnlyList<CustomerQueryResponse>>
{

}
