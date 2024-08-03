using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApp.Repositories
{
    internal class TaskProjectRepository
    {
        private readonly ApplicationContext _applicationContext;

        public TaskProjectRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<TaskProject>> GetAll()
        {
            return await _applicationContext.Tasks
                .AsNoTracking()
                .OrderBy(t => t.StartDate)
                .ToListAsync();
        }

        public async Task<TaskProject?> GetById(int taskProjectId)
        {
            return await _applicationContext.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TaskProjectId == taskProjectId);
        }

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

        public async Task Add(string name, string description, DateTime startDate, DateTime endDate, int projectId, int assignedUserId)
        {
            var taskProject = new TaskProject(name, description, startDate, endDate, projectId, assignedUserId);
            await _applicationContext.AddAsync(taskProject);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task Update(int taskProjectId, string name, string description, DateTime startDate, DateTime endDate, int projectId, int assignedUserId)
        {
            await _applicationContext.Tasks
                .Where(t => t.TaskProjectId == taskProjectId)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(t => t.Name, name)
                    .SetProperty(t => t.Description, description)
                    .SetProperty(t => t.StartDate, startDate)
                    .SetProperty(t => t.EndDate, endDate)
                    .SetProperty(t => t.ProjectId, projectId)
                    .SetProperty(t => t.AssignedUserId, assignedUserId));
        }

        public async Task Delete(int taskProjectId)
        {
            await _applicationContext.Tasks
                .Where(t => t.TaskProjectId == taskProjectId)
                .ExecuteDeleteAsync();
        }
    }
}
