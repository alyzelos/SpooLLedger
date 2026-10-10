using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Domain;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController(DataContext dataContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilamentStock>>> GetFilament()
        {
            var stocks = await dataContext.FilamentStocks.ToListAsync();
            if( stocks == null) return NotFound("no filaments");

            return stocks;
        }
    }
}
