using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Repositories.Base;

namespace ProjectManagerApp.Repositories
{
    internal class ReportRepository : RepositoryBase<Report>
    {
        public ReportRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<List<Report>> GetByFilterProject(int projectId)
        {
            return await _applicationContext.Reports
                .AsNoTracking()
                .Where(r => r.ProjectId == projectId)
                .ToListAsync();
        }
    }
}
