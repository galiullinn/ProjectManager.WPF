using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagerApp.Repositories
{
    internal class ReportRepository
    {
        private readonly ApplicationContext _applicationContext;

        public ReportRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Report>> GetAll()
        {
            return await _applicationContext.Reports
                .AsNoTracking()
                .OrderBy(r => r.ReportId)
                .ToListAsync();
        }

        public async Task<Report?> GetById(int reportId)
        {
            return await _applicationContext.Reports
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task<List<Report>> GetByFilterProject(int projectId)
        {
            return await _applicationContext.Reports
                .AsNoTracking()
                .Where(r => r.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task Add(string title, string content, DateTime dateCreate, int projectId)
        {
            Report report = new Report(title, content, projectId);

            await _applicationContext.AddAsync(report);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task Update(int reportId, string title, string content, DateTime dateCreate, int projectId)
        {
            await _applicationContext.Reports
                .Where(r => r.ReportId == reportId)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(r => r.Title, title)
                    .SetProperty(r => r.Content, content)
                    .SetProperty(r => r.DateCreate, dateCreate)
                    .SetProperty(r => r.ProjectId, projectId));
        }

        public async Task Delete(int reportId)
        {
            await _applicationContext.Reports
                .Where(t => t.ReportId == reportId)
                .ExecuteDeleteAsync();
        }
    }
}
