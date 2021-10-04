using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Data
{
    public sealed class QueryFluent<TEntity> where TEntity : class , IObjectState
    {
        private readonly List<Expression<Func<TEntity, object>>> _includeProperties;

        private readonly Repository<TEntity> _repository ;
        private Expression<Func<TEntity, bool>> _filter;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderByQuerable;
        private int? _page;
        private int? _pageSize;

        public QueryFluent(Repository<TEntity> repository)
        {
            _repository = repository;
            _includeProperties = new List<Expression<Func<TEntity, object>>>();
        }


        public QueryFluent<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public QueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public QueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public IEnumerable<TEntity> Get(out int totalRecords, int? page, int? pageSize)
        {
            _page = page;
            _pageSize = pageSize;
            totalRecords = _repository.Get(_filter).Count();

            return _repository.Get(
                _filter,
                _orderByQuerable, _includeProperties, _page, _pageSize);
        }

        public IEnumerable<TEntity> Get()
        {
            return _repository.Get(
                _filter,
                _orderByQuerable, _includeProperties, _page, _pageSize);
        }
    }
}
