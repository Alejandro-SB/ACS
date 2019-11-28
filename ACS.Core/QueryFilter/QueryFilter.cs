using ACS.Core.Entities.Base;
using ACS.Core.Interfaces.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ACS.Core.QueryFilter
{
    public class QueryFilter<T> : IQueryFilter<T>
        where T : BaseEntity
    {
        private readonly List<Expression<Func<T, object>>> _includes = new List<Expression<Func<T, object>>>();

        public QueryFilter(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        public Expression<Func<T, bool>> Predicate { get; }

        public IReadOnlyCollection<Expression<Func<T, object>>> Includes => _includes.AsReadOnly();

        protected void Include(Expression<Func<T, object>> include)
        {
            _includes.Add(include);
        }
    }
}