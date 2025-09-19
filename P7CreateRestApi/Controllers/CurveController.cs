using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly LocalDbContext _context;

        // Inject Curve Point service
        public CurveController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: api/curve
        [HttpGet]
        [Authorize] // accessible aux utilisateurs connectés
        public async Task<IActionResult> GetAll()
        {
            //  find all CurvePoint, add to model
            var curves = await _context.CurvePoints.ToListAsync();
            return Ok(curves);
        }

        // GET: api/curve/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            //  get CurvePoint by Id and to model then show to the form
            var curve = await _context.CurvePoints.FindAsync(id);
            if (curve == null)
                return NotFound();

            return Ok(curve);
        }

        // POST: api/curve
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CurvePoint curvePoint)
        {
            //  check data valid and save to db, after saving return CurvePoint list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = curvePoint.Id }, curvePoint);
        }

        // PUT: api/curve/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
        {
            // check required fields, if valid call service to update Curve and return Curve list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.CurvePoints.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Mise à jour des champs
            existing.Term = curvePoint.Term;
            existing.Value = curvePoint.Value;
            existing.AsOfDate = curvePoint.AsOfDate;
            existing.CreationDate = curvePoint.CreationDate ?? existing.CreationDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/curve/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Find Curve by Id and delete the Curve, return to Curve list
        public async Task<IActionResult> Delete(int id)
        {
            var curve = await _context.CurvePoints.FindAsync(id);
            if (curve == null)
                return NotFound();

            _context.CurvePoints.Remove(curve);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
