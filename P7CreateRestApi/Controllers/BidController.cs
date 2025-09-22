using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public BidController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: api/bid
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var bids = await _context.Bids.ToListAsync();
            return Ok(bids);
        }

        // GET: api/bid/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid == null)
                return NotFound();

            return Ok(bid);
        }

        // POST: api/bid
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Bid bid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = bid.Id }, bid);
        }

        // PUT: api/bid/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Bid bid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.Bids.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Mise à jour des champs principaux
            existing.Account = bid.Account;
            existing.Type = bid.Type;
            existing.BidQuantity = bid.BidQuantity;
            existing.AskQuantity = bid.AskQuantity;
            existing.BidPrice = bid.BidPrice;
            existing.AskPrice = bid.AskPrice;
            existing.Benchmark = bid.Benchmark;
            existing.Date = bid.Date;
            existing.Commentary = bid.Commentary;
            existing.Security = bid.Security;
            existing.Status = bid.Status;
            existing.Trader = bid.Trader;
            existing.Book = bid.Book;
            existing.CreationName = bid.CreationName;
            existing.CreationDate = bid.CreationDate ?? existing.CreationDate;
            existing.RevisionName = bid.RevisionName;
            existing.RevisionDate = bid.RevisionDate ?? existing.RevisionDate;
            existing.DealName = bid.DealName;
            existing.DealType = bid.DealType;
            existing.SourceListId = bid.SourceListId;
            existing.Side = bid.Side;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/bid/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid == null)
                return NotFound();

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
