using ForexFlow.DataAccess.Database;
using ForexFlow.DataAccess.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace ForexFlow.DataAccess.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dataBaseSet;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this.dataBaseSet = this._context.Set<T>();
        }

        public void Add(T entity)
        {
            this.dataBaseSet.Add(entity);
        }

        public void Delete(T entity)
        {
            this.dataBaseSet.Remove(entity);
        }

        public async Task<T?> Get(Expression<Func<T, bool>>? whereClause = null, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> entityQuery;
            if (tracked)
            {
                entityQuery = this.dataBaseSet;
            }
            else
            {
                entityQuery = this.dataBaseSet.AsNoTracking();
            }

            if (whereClause != null)
            {
                entityQuery = entityQuery.Where(whereClause);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    entityQuery = entityQuery.Include(includeProperties);
                }
            }

            return await entityQuery.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? whereClause = null, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> entityQuery = this.dataBaseSet;

            if (tracked)
            {
                entityQuery = this.dataBaseSet;
            }
            else
            {
                entityQuery = this.dataBaseSet.AsNoTracking();
            }

            if (whereClause != null)
            {
                entityQuery = entityQuery.Where(whereClause);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    entityQuery = entityQuery.Include(includeProperties);
                }
            }

            return await entityQuery.ToListAsync();
        }
    }
}
