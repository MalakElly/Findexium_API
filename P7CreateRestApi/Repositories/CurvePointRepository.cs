using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository
    {
        private readonly LocalDbContext _dbContext;

        public CurvePointRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CurvePoint>> FindAllAsync() => await _dbContext.CurvePoints.ToListAsync();

        public async Task<CurvePoint?> FindByIdAsync(int id) => await _dbContext.CurvePoints.FindAsync(id);

        public async Task AddAsync(CurvePoint curvePoint)
        {
            _dbContext.CurvePoints.Add(curvePoint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CurvePoint curvePoint)
        {
            _dbContext.CurvePoints.Update(curvePoint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CurvePoint curvePoint)
        {
            _dbContext.CurvePoints.Remove(curvePoint);
            await _dbContext.SaveChangesAsync();
        }
    }
}
