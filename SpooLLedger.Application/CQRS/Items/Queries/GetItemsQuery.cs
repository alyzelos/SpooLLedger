using System;
using MediatR;
using SpooLLedger.Application.CQRS.Items.Responses;

namespace SpooLLedger.Application.CQRS.Items.Queries;

public class GetItemsQuery : IRequest<IReadOnlyList<GetItemResponse>>
{

}
