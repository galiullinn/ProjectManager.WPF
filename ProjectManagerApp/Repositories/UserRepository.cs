using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApp.Repositories
{
    internal class UserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<User>> GetAll()
        {
            return await _applicationContext.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetById(int userId)
        {
            return await _applicationContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<List<User>> GetByFilterPosition(Position position)
        {
            var query = _applicationContext.Users
                .AsNoTracking()
                .Where(u => u.Position == position);

            return await query.ToListAsync();
        }
        public async Task Add(string firstName, string lastName, string email, string phone, Position position)
        {
            var user = new User(firstName, lastName, email, phone, position);
            await _applicationContext.AddAsync(user);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task Update(int userId, string firstName, string lastName, string email, string phone, Position position)
        {
            await _applicationContext.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.FirstName, firstName)
                    .SetProperty(u => u.LastName, lastName)
                    .SetProperty(u => u.Email, email)
                    .SetProperty(u => u.Phone, phone)
                    .SetProperty(u => u.Position, position));
        }
        public async Task Delete(int userId)
        {
            await _applicationContext.Users
                .Where(u => u.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
