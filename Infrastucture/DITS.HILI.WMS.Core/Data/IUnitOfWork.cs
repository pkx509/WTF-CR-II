using System;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        DbContextScope ContextScope { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepository<T> Repository<T>() where T : class;
        DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters);



    }
}
