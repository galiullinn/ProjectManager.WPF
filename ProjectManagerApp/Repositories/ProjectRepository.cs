using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApp.Repositories
{
    internal class ProjectRepository
    {
        private readonly ApplicationContext _applicationContext;

        public ProjectRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Project>> GetAll()
        {
            return await _applicationContext.Projects
                .AsNoTracking()
                .OrderBy(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<Project?> GetById(int projectId)
        {
            return await _applicationContext.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        public async Task Add(string name, string description, DateTime startDate, DateTime endDate)
        {
            var project = new Project(name, description, startDate, endDate);
            await _applicationContext.AddAsync(project);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task Update(int projectId, string name, string description, DateTime startDate, DateTime endDate)
        {
            await _applicationContext.Projects
                .Where(p => p.ProjectId == projectId)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, name)
                    .SetProperty(p => p.Description, description)
                    .SetProperty(p => p.StartDate, startDate)
                    .SetProperty(p => p.EndDate, endDate));
        }

        public async Task Delete(int projectId)
        {
            await _applicationContext.Projects
                .Where(p => p.ProjectId == projectId)
                .ExecuteDeleteAsync();
        }
    }
}
