using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly LocalDbContext _context;

        // Inject RuleName service
        public RuleNameController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: api/rulename
        [HttpGet]
        [Authorize] //find all RuleName, add to model
        public async Task<IActionResult> GetAll()
        {
            var rules = await _context.RuleNames.ToListAsync();
            return Ok(rules);
        }

        // GET: api/rulename/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var rule = await _context.RuleNames.FindAsync(id);
            if (rule == null)
                return NotFound();

            return Ok(rule);
        }

        // POST: api/rulename
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] RuleName rule)
        {
            //check data valid and save to db, after saving return RuleName list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.RuleNames.Add(rule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rule.Id }, rule);
        }

        // PUT: api/rulename/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] RuleName rule)
        {
            //check required fields, if valid call service to update RuleName and return RuleName list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.RuleNames.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Mise à jour des champs
            existing.Name = rule.Name;
            existing.Description = rule.Description;
            existing.Json = rule.Json;
            existing.Template = rule.Template;
            existing.SqlStr = rule.SqlStr;
            existing.SqlPart = rule.SqlPart;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/rulename/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] //Find RuleName by Id and delete the RuleName, return to Rule list
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _context.RuleNames.FindAsync(id);
            if (rule == null)
                return NotFound();

            _context.RuleNames.Remove(rule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
