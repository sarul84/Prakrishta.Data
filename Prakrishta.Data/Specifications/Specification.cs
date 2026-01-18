namespace Prakrishta.Data.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; protected set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }
        public IList<Expression<Func<T, object>>>? IncludeProperties { get; protected set; } = new List<Expression<Func<T, object>>>();
        public int? PageNumber { get; protected set; }
        public int? PageSize { get; protected set; }
        public bool IsPagingEnabled { get; protected set; }
        public bool AsNoTracking { get; protected set; } = true;


        protected void AddInclude(Expression<Func<T, object>> includeProperty)
        {
            if (IncludeProperties is null)
            {
                throw new InvalidOperationException("IncludeProperties collection is null.");
            }

            IncludeProperties.Add(includeProperty);
        }

        protected void ApplyPaging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            IsPagingEnabled = true;
        }

        protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression) => OrderBy = orderByExpression;

        protected void TrackChanges() => AsNoTracking = false;

    }
}
