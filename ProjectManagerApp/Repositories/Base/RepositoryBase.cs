using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Data;

namespace ProjectManagerApp.Repositories.Base
{
    internal abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _applicationContext;

        protected RepositoryBase(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _applicationContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await _applicationContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual async Task Add(T entity)
        {
            await _applicationContext.AddAsync(entity);
            await _applicationContext.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            _applicationContext.Update(entity);
            await _applicationContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _applicationContext.Remove(entity);
                await _applicationContext.SaveChangesAsync();
            }
        }
    }
}
