using MediatR;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Queries.GetPricings;

public record GetPricingsQuery : IRequest<IReadOnlyList<PricingDto>>;
