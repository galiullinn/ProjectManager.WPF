using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Repositories.Base;

namespace ProjectManagerApp.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment>
    {
        public CommentRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<List<Comment>> GetByFilterTaskProject(int taskProjectId)
        {
            return await _applicationContext.Comments
                .AsNoTracking()
                .Where(c => c.TaskProjectId == taskProjectId)
                .ToListAsync();
        }
        public async Task<List<Comment>> GetByFilterUser(int userId)
        {
            return await _applicationContext.Comments
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
