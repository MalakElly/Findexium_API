using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingRepository
    {
        private readonly LocalDbContext _dbContext;

        public RatingRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Rating>> FindAllAsync() => await _dbContext.Ratings.ToListAsync();

        public async Task<Rating?> FindByIdAsync(int id) => await _dbContext.Ratings.FindAsync(id);

        public async Task AddAsync(Rating rating)
        {
            _dbContext.Ratings.Add(rating);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rating rating)
        {
            _dbContext.Ratings.Update(rating);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rating rating)
        {
            _dbContext.Ratings.Remove(rating);
            await _dbContext.SaveChangesAsync();
        }
    }
}
