using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tout le contrôleur nécessite un utilisateur connecté
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _repository;

        public TradeController(TradeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trades = await _repository.FindAllAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trade = await _repository.FindByIdAsync(id);
            if (trade == null) return NotFound();
            return Ok(trade);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Trade trade)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repository.AddAsync(trade);
            return CreatedAtAction(nameof(GetById), new { id = trade.TradeId }, trade);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repository.FindByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Account = trade.Account;
            existing.Type = trade.Type;
            existing.BuyQuantity = trade.BuyQuantity;
            existing.SellQuantity = trade.SellQuantity;
            existing.BuyPrice = trade.BuyPrice;
            existing.SellPrice = trade.SellPrice;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var trade = await _repository.FindByIdAsync(id);
            if (trade == null) return NotFound();

            await _repository.DeleteAsync(trade);
            return NoContent();
        }
    }
}
