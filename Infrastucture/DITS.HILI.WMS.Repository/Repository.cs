using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IObjectState
    {
        #region [ Property ]
        internal readonly IDbContext _context;
        internal DbSet<TEntity> _dbSet;
        #endregion

        #region [ Contructor ]
        public Repository(IDbContext context)
        {
            _context = context;
            var dbContext = context as DbContext;
            _dbSet = dbContext.Set<TEntity>();
        }


        #endregion

        #region [ Method ]

        public virtual void Add(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            _context.SaveChanges();

        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.ObjectState = ObjectState.Added;
                _dbSet.Attach(entity);
                _context.SyncObjectState(entity);
            }
            _context.SaveChanges();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual TEntity GetByID(object id)
        {
            var entity = _dbSet.Find(id);
            return entity;
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            var entity = _dbSet.Find(keyValues);
            return entity;
        }

        public virtual void Modify(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            _context.SaveChanges();
        }


        public virtual void Remove(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            _context.SaveChanges();
        }

        public virtual void Remove(object id)
        {
            var entity = _dbSet.Find(id);
            Remove(entity);
        }

        public QueryFluent<TEntity> Query()
        {
            var repositoryGetFluentHelper = new QueryFluent<TEntity>(this);
            return repositoryGetFluentHelper;
        }

        internal IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                     List<Expression<Func<TEntity, object>>> includeProperties = null, int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            page = (page == 0 ? null : page);
            pageSize = (pageSize == 0 ? null : pageSize);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query.AsNoTracking();
        }


        #endregion
         
    }
}
