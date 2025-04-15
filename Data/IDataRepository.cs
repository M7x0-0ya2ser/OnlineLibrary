using System.Linq.Expressions;

namespace OnlineLibrary.Data
{
    public interface IDataRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        Task<T> GetByIdAsync(object id);

        IQueryable<T> GetQueryable();

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();

    }
}
