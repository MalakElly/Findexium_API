using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public TradeController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: api/trade
        [HttpGet]
        [Authorize] //accessible aux utilisateurs connectés
        public async Task<IActionResult> GetAll()
        {
            var trades = await _context.Trades.ToListAsync();
            return Ok(trades);
        }

        // GET: api/trade/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
                return NotFound();

            return Ok(trade);
        }

        // POST: api/trade
        [HttpPost]
        [Authorize] //tout utilisateur connecté peut créer une transaction
        public async Task<IActionResult> Create([FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = trade.TradeId }, trade);
        }

        // PUT: api/trade/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.Trades.FindAsync(id);
            if (existing == null)
                return NotFound();

            //Mise à jour des champs
            existing.Account = trade.Account;
            existing.Type = trade.Type;
            existing.BuyQuantity = trade.BuyQuantity;
            existing.BuyPrice = trade.BuyPrice;
            existing.SellQuantity = trade.SellQuantity;
            existing.SellPrice = trade.SellPrice;
            existing.TradeDate = trade.TradeDate ?? existing.TradeDate;
            existing.Security = trade.Security;
            existing.Status = trade.Status;
            existing.Trader = trade.Trader;
            existing.Benchmark = trade.Benchmark;
            existing.Book = trade.Book;
            existing.CreationName = trade.CreationName;
            existing.CreationDate = trade.CreationDate ?? existing.CreationDate;
            existing.RevisionName = trade.RevisionName;
            existing.RevisionDate = trade.RevisionDate ?? existing.RevisionDate;
            existing.DealName = trade.DealName;
            existing.DealType = trade.DealType;
            existing.SourceListId = trade.SourceListId;
            existing.Side = trade.Side;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/trade/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] //suppression réservée aux ADMINs
        public async Task<IActionResult> Delete(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
                return NotFound();

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
