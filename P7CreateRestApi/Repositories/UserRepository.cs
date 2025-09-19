using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        private readonly LocalDbContext _dbContext;

        public UserRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Récupérer un utilisateur par username
        public async Task<User?> FindByUserNameAsync(string userName)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        // Récupérer tous les utilisateurs
        public async Task<List<User>> FindAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        // Ajouter un nouvel utilisateur
        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        // Récupérer un utilisateur par ID
        public async Task<User?> FindByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Mettre à jour un utilisateur
        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        // Supprimer un utilisateur
        public async Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
