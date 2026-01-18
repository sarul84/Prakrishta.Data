using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Prakrishta.Data.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
        IList<Expression<Func<T, object>>>? IncludeProperties { get; }
        int? PageNumber { get; }
        int? PageSize { get; }
        bool IsPagingEnabled { get; }
        bool AsNoTracking { get; }
    }
}
