using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DbContextScope _context;
        private Hashtable _repositories;

        private bool _disposed;

        public DbContextScope ContextScope => _context;

        public UnitOfWork(string connectionString)
        {
            _context = new DbContextScope(connectionString);
            _context.Database.CommandTimeout = 0;
        }

        public int SaveChanges()
        {
            int changes = _context.SaveChanges();
            return changes;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            int changesAsync = await _context.SaveChangesAsync(cancellationToken);
            return changesAsync;
        }


        protected void Dispose(bool disposing)
        {

            if (!_disposed)
            {
                if (disposing)
                {

                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            string type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                Type repositoryType = typeof(Repository<>);

                object repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), this);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
        public IEnumerable<T> GetWithRawSql<T>(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters).ToList();
        }

        public DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters);
        }
    }
}
