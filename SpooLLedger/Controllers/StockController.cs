using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Data.DatabaseContext;
using SpooLLedger.Entities;

namespace SpooLLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController(DataContext dataContext) : ControllerBase
    {
        public async Task<ActionResult<IEnumerable<FilamentStock>>> GetFilament()
        {
            var stocks = await dataContext.FilamentStocks.ToListAsync();
            if( stocks == null) return NotFound("no filaments");

            return stocks;
        }
    }
}
