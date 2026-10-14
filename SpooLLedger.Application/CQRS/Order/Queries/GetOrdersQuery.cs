using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrdersQuery : IRequest<IReadOnlyList<OrderQueryResponse>>;
