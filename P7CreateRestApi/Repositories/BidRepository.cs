using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository
    {
        private readonly LocalDbContext _dbContext;

        public BidRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Bid>> FindAllAsync() => await _dbContext.Bids.ToListAsync();

        public async Task<Bid?> FindByIdAsync(int id) => await _dbContext.Bids.FindAsync(id);

        public async Task AddAsync(Bid bid)
        {
            _dbContext.Bids.Add(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bid bid)
        {
            _dbContext.Bids.Update(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bid bid)
        {
            _dbContext.Bids.Remove(bid);
            await _dbContext.SaveChangesAsync();
        }
    }
}
