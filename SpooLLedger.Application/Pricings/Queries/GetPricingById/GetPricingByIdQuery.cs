using MediatR;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Queries.GetPricingById;

public record GetPricingByIdQuery(int Id) : IRequest<PricingDto?>;
