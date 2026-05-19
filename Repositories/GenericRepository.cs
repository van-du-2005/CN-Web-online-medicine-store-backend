using Microsoft.EntityFrameworkCore;
using OnlineMedicineStoreBackend.Data;
namespace OnlineMedicineStoreBackend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly OnlineMedicineStoreCNWDbContext _Context;
        protected readonly DbSet<T> _DbSet;

        public GenericRepository(OnlineMedicineStoreCNWDbContext context)
        {
            _Context = context;
            _DbSet = _Context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _DbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _DbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _DbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _DbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _DbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}