using MediatR;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Queries.GetFilamentStockById;

public record GetFilamentStockByIdQuery(int Id) : IRequest<FilamentStockDto?>;
