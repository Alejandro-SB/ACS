using ACS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ACS.Core.Interfaces.Filters
{
    public interface IQueryFilter<T>
        where T: BaseEntity
    {
        Expression<Func<T, bool>> Predicate { get; }
        IReadOnlyCollection<Expression<Func<T, object>>> Includes { get; }
    }
}