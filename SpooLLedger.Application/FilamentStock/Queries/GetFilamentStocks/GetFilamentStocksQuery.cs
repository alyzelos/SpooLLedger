using MediatR;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Queries.GetFilamentStocks;

public record GetFilamentStocksQuery : IRequest<IReadOnlyList<FilamentStockDto>>;
