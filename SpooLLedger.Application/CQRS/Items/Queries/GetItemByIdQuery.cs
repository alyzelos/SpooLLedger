using System;
using MediatR;
using SpooLLedger.Application.CQRS.Items.Responses;

namespace SpooLLedger.Application.CQRS.Items.Queries;

public class GetItemByIdQuery : IRequest<GetItemResponse>
{
    public GetItemByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }

}
