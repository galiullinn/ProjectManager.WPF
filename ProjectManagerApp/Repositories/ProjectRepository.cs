using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Repositories.Base;

namespace ProjectManagerApp.Repositories
{
    internal class ProjectRepository : RepositoryBase<Project>
    {
        public ProjectRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
