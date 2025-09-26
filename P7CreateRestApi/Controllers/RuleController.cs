using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // connecté requis
    public class RuleController : ControllerBase
    {
        private readonly RuleRepository _repository;

        public RuleController(RuleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rules = await _repository.FindAllAsync();
            return Ok(rules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rule = await _repository.FindByIdAsync(id);
            if (rule == null) return NotFound();
            return Ok(rule);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Rule rule)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repository.AddAsync(rule);
            return CreatedAtAction(nameof(GetById), new { id = rule.Id }, rule);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] Rule rule)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _repository.FindByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = rule.Name;
            existing.Description = rule.Description;
            existing.Json = rule.Json;
            existing.Template = rule.Template;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _repository.FindByIdAsync(id);
            if (rule == null) return NotFound();

            await _repository.DeleteAsync(rule);
            return NoContent();
        }
    }
}
