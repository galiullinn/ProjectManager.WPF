using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApp.Repositories
{
    internal class CommentRepository
    {
        private readonly ApplicationContext _applicationContext;

        public CommentRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Comment>> GetAll()
        {
            return await _applicationContext.Comments
                .AsNoTracking()
                .OrderBy(c => c.CommentId)
                .ToListAsync();
        }

        public async Task<Comment?> GetById(int commentId)
        {
            return await _applicationContext.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

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

        public async Task Add(string text, int taskProject, int userId)
        {
            var report = new Comment(text, taskProject, userId);

            await _applicationContext.AddAsync(report);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task Update(int commentId, string text, DateTime dateCreate, int taskProjectId, int userId)
        {
            await _applicationContext.Comments
                .Where(c => c.CommentId == commentId)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(c => c.Text, text)
                    .SetProperty(c => c.DateCreate, dateCreate)
                    .SetProperty(c => c.TaskProjectId, taskProjectId)
                    .SetProperty(c => c.UserId, userId));
        }

        public async Task Delete(int commentId)
        {
            await _applicationContext.Comments
                .Where(c => c.CommentId == commentId)
                .ExecuteDeleteAsync();
        }
    }
}
