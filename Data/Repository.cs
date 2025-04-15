
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OnlineLibrary.Data
{
    public class Repository<T> : IDataRepository<T>
        where T : class
    {
        protected readonly OnlineLibraryContext _context;
        private readonly DbSet<T> table;

        public Repository(OnlineLibraryContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public Repository()
        {

        }
        public void Delete(T entity)
        {
            table.Remove(entity);   
        }
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query =  table;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }
        

        public async Task<T> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }

        public IQueryable<T> GetQueryable()
        {
            return table;
        }

        public void Insert(T entity)
        {
            table.Add(entity);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            table.Update(entity);   
        }

    }
}
