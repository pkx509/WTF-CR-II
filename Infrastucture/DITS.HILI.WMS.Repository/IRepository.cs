using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Data
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {
        TEntity GetByID(object id); 
        TEntity Find(params object[] keyValues);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Modify(TEntity entity); 
        void Remove(object id);
        void Remove(TEntity entity);
        void Dispose();
        QueryFluent<TEntity> Query();
    }
}
