using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // accès réservé aux utilisateurs connectés
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _repository;

        public RatingController(RatingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ratings = await _repository.FindAllAsync();
            return Ok(ratings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _repository.FindByIdAsync(id);
            if (rating == null) return NotFound();
            return Ok(rating);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Rating rating)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repository.AddAsync(rating);
            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repository.FindByIdAsync(id);
            if (existing == null) return NotFound();

            existing.MoodysRating = rating.MoodysRating;
            existing.SandPRating = rating.SandPRating;
            existing.FitchRating = rating.FitchRating;
            existing.OrderNumber = rating.OrderNumber;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var rating = await _repository.FindByIdAsync(id);
            if (rating == null) return NotFound();

            await _repository.DeleteAsync(rating);
            return NoContent();
        }
    }
}
