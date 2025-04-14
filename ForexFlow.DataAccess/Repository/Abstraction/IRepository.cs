using System.Linq.Expressions;

namespace ForexFlow.DataAccess.Repository.Abstraction
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? whereClause = null, string? includeProperties = null, bool tracked = false);
        public Task<T?> Get(Expression<Func<T, bool>>? whereClause = null, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Delete(T entity);
    }
}
