using DITS.HILI.Framework;
using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DITS.HILI.WMS.Core.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region [ Property ]

        public IUnitOfWork Context { get; private set; }

        public DbSet<TEntity> DbSet { get; private set; }

        protected void GetProductInfo<T>(System.Reflection.Assembly assambly)
        {
            Dictionary<string, object> info = Utilities.GetProductInfo<T>(assambly);
            Version = info["Version"].ToString();
            InstanceID = Guid.Parse(info["ID"].ToString());
        }

        public Guid InstanceID { get; set; }
        public string Version { get; set; }

        private Guid _userID;
        public Guid UserID
        {
            get => _userID;

            set => _userID = value;
        }

        #endregion

        #region [ Contructor ]
        public Repository(IUnitOfWork context)
        {
            Context = context;
            DbSet = Context.ContextScope.Set<TEntity>();
        }

        #endregion

        #region [ Method ]

        public virtual TEntity Add(TEntity entity)
        {
            Context.ContextScope.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {

            foreach (TEntity entity in entities)
            {
                Context.ContextScope.Entry(entity).State = EntityState.Added;
            }
            Context.SaveChanges();
            return entities;
        }

        public virtual TEntity FindByID(object id)
        {
            TEntity entity = DbSet.Find(id);
            return entity;
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            TEntity entity = DbSet.Find(keyValues);
            return entity;
        }

        public virtual void Modify(TEntity entity)
        {
            Context.ContextScope.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Modify(TEntity current, TEntity entity)
        {
            Context.ContextScope.Entry(entity).CurrentValues.SetValues(current);
            Context.SaveChanges();
        }

        public virtual void Remove(TEntity entity)
        {
            Context.ContextScope.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public virtual void Remove(object id)
        {
            TEntity entity = DbSet.Find(id);
            Remove(entity);
        }

        public virtual void Remove(object id, params object[] param)
        {
        }

        public virtual bool OnReceiveData(List<DataTransfer> data)
        {

            return true;
        }

        public RepositoryQuery<TEntity> Query()
        {
            return new RepositoryQuery<TEntity>(this);
        }

        public IQueryable<TEntity> Where()
        {
            return Where(null, null, null, null, null);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Where(predicate, null, null, null, null);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return Where(predicate, orderBy, null, null, null);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, List<Expression<Func<TEntity, object>>> includeProperties)
        {
            return Where(predicate, orderBy, includeProperties, null, null);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, List<Expression<Func<TEntity, object>>> includeProperties = null, int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet; 
            if (includeProperties != null)
            {
                includeProperties.ForEach(i => { query = query.Include(i); });
            } 
            if (predicate != null)
            {
                query = query.Where(predicate);
            } 
            if (orderBy != null)
            {
                query = orderBy(query);
            } 
            if (page.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            } 
            return query.AsNoTracking();
        }

        public bool Any()
        {
            return Any(null);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.Any();
        }

        public TEntity FirstOrDefault()
        {
            return FirstOrDefault(null, null);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return FirstOrDefault(predicate, null);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.FirstOrDefault();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return SingleOrDefault(predicate, null);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.SingleOrDefault();
        }
        public TEntity Clone<TEntity>(TEntity source)
        {
            if (!typeof(TEntity).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(TEntity);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (TEntity)formatter.Deserialize(stream);
            }
        }


        #endregion

        public TEntity Clone<T>(TEntity source)
        {
            if (!typeof(T).IsSerializable)
            {
                return default(TEntity);
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(TEntity);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (TEntity)formatter.Deserialize(stream);
            }
        }
    }
}
