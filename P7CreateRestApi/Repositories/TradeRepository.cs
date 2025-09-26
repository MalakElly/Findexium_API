using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository
    {
        private readonly LocalDbContext _dbContext;

        public TradeRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Trade>> FindAllAsync() => await _dbContext.Trades.ToListAsync();

        public async Task<Trade?> FindByIdAsync(int id) => await _dbContext.Trades.FindAsync(id);

        public async Task AddAsync(Trade trade)
        {
            _dbContext.Trades.Add(trade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trade trade)
        {
            _dbContext.Trades.Update(trade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Trade trade)
        {
            _dbContext.Trades.Remove(trade);
            await _dbContext.SaveChangesAsync();
        }
    }
}
