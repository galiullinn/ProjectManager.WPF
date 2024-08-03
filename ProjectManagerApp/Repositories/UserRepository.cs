using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Repositories.Base;

namespace ProjectManagerApp.Repositories
{
    internal class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<List<User>> GetByFilterPosition(Position position)
        {
            var query = _applicationContext.Users
                .AsNoTracking()
                .Where(u => u.Position == position);

            return await query.ToListAsync();
        }
    }
}
