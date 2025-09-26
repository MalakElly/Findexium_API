using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // nécessite d’être connecté
    public class CurvePointController : ControllerBase
    {
        private readonly CurvePointRepository _repository;

        public CurvePointController(CurvePointRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var curves = await _repository.FindAllAsync();
            return Ok(curves);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curve = await _repository.FindByIdAsync(id);
            if (curve == null) return NotFound();
            return Ok(curve);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CurvePoint curvePoint)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repository.AddAsync(curvePoint);
            return CreatedAtAction(nameof(GetById), new { id = curvePoint.Id }, curvePoint);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repository.FindByIdAsync(id);
            if (existing == null) return NotFound();

            existing.CurveId = curvePoint.CurveId;
            existing.Term = curvePoint.Term;
            existing.Value = curvePoint.Value;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var curve = await _repository.FindByIdAsync(id);
            if (curve == null) return NotFound();

            await _repository.DeleteAsync(curve);
            return NoContent();
        }
    }
}
