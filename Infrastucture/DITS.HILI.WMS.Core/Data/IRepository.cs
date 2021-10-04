using DITS.HILI.WMS.Core.Infrastructure.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DITS.HILI.WMS.Core.Data
{
    public interface IRepository<TEntity> : IWorkEngine where TEntity : class
    {
        IUnitOfWork Context { get; }
        DbSet<TEntity> DbSet { get; }
        Guid UserID { get; set; }
        TEntity FindByID(object id);
        TEntity Find(params object[] keyValues);
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        void Modify(TEntity entity);
        void Modify(TEntity current, TEntity entity);
        void Remove(object id);
        void Remove(object id, params object[] param);
        void Remove(TEntity entity);
        RepositoryQuery<TEntity> Query();

        IQueryable<TEntity> Where();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        IQueryable<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            List<Expression<Func<TEntity, object>>> includeProperties);


        IQueryable<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null);

        bool Any();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
    }
}
