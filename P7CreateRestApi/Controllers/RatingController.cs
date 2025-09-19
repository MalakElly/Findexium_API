using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Controllers.Domain;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly LocalDbContext _context;

        // Inject Rating service
        public RatingController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: api/rating
        [HttpGet]
        [Authorize] // accessible aux utilisateurs connectés
        public async Task<IActionResult> GetAll()
        {
            // find all Rating, add to model
            var ratings = await _context.Ratings.ToListAsync();
            return Ok(ratings);
        }

        // GET: api/rating/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            //get Rating by Id and to model then show to the form
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound();

            return Ok(rating);
        }

        // POST: api/rating
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Rating rating)
        {
            //check data valid and save to db, after saving return Rating list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
        }

        // PUT: api/rating/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
        {
            //check required fields, if valid call service to update Rating and return Rating list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.Ratings.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Mise à jour des champs
            existing.MoodysRating = rating.MoodysRating;
            existing.SandPRating = rating.SandPRating;
            existing.FitchRating = rating.FitchRating;
            existing.OrderNumber = rating.OrderNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/rating/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // suppression réservée aux ADMIN
        public async Task<IActionResult> Delete(int id)
        {
            // Find Rating by Id and delete the Rating, return to Rating list
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound();

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
