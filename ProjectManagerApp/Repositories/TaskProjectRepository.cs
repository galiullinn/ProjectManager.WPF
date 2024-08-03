using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Repositories.Base;

namespace ProjectManagerApp.Repositories
{
    internal class TaskProjectRepository : RepositoryBase<TaskProject>
    {
        public TaskProjectRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<List<TaskProject>> GetByFilterProject(int projectId)
        {
            return await _applicationContext.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<TaskProject>> GetByFilterManager(int assignedUserId)
        {
            return await _applicationContext.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedUserId == assignedUserId)
                .ToListAsync();
        }
    }
}
