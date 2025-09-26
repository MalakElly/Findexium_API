using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class RuleRepository
    {
        private readonly LocalDbContext _dbContext;

        public RuleRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Rule>> FindAllAsync() => await _dbContext.Rules.ToListAsync();

        public async Task<Rule?> FindByIdAsync(int id) => await _dbContext.Rules.FindAsync(id);

        public async Task AddAsync(Rule rule)
        {
            _dbContext.Rules.Add(rule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rule rule)
        {
            _dbContext.Rules.Update(rule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rule ruleName)
        {
            _dbContext.Rules.Remove(ruleName);
            await _dbContext.SaveChangesAsync();
        }
    }
}
