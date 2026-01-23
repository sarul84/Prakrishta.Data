//----------------------------------------------------------------------------------
// <copyright file="Specification.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/19/2026</date>
// <summary>Abstraction class of Specification contract</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Gets or sets the Criteria.
        /// </summary>
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        /// <summary>
        /// Gets or sets the Order By.
        /// </summary>
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }

        /// <summary>
        /// Gets or sets the Include Properties.
        /// </summary>
        public IList<Expression<Func<T, object>>>? IncludeProperties { get; protected set; } = [];

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? PageNumber { get; protected set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? PageSize { get; protected set; }

        /// <summary>
        /// Gets or sets Is Paging Enable flag.
        /// </summary>
        public bool IsPagingEnabled { get; protected set; }

        /// <summary>
        /// Gets or sets the AsNoTracking flag.
        /// </summary>
        public bool AsNoTracking { get; protected set; } = true;

        /// <summary>
        /// Adds a property expression to the list of related entities to include in query results.
        /// </summary>
        /// <remarks>Use this method to specify related entities that should be included in the query
        /// results, enabling eager loading of navigation properties.</remarks>
        /// <param name="includeProperty">An expression that specifies the related property to include. Typically a lambda expression identifying a
        /// navigation property to be eagerly loaded.</param>
        /// <exception cref="InvalidOperationException">Thrown if the IncludeProperties collection is null.</exception>
        protected void AddInclude(Expression<Func<T, object>> includeProperty)
        {
            if (IncludeProperties is null)
            {
                throw new InvalidOperationException("IncludeProperties collection is null.");
            }

            IncludeProperties.Add(includeProperty);
        }

        /// <summary>
        /// Enables paging by setting the current page number and page size.
        /// </summary>
        /// <param name="pageNumber">The one-based index of the page to display. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items to include on each page. Must be greater than 0.</param>
        protected void ApplyPaging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Applies the specified ordering expression to the underlying queryable data source.
        /// </summary>
        /// <remarks>Use this method to specify the sort order for queries before executing them. Calling
        /// this method replaces any previously applied ordering expression.</remarks>
        /// <param name="orderByExpression">A function that defines the ordering of the queryable data source. The function should take an <see
        /// cref="IQueryable{T}"/> and return an <see cref="IOrderedQueryable{T}"/> representing the desired order.</param>
        protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression) => OrderBy = orderByExpression;

        /// <summary>
        /// Applies the No Tracking behavior to the query results.
        /// </summary>
        protected void TrackChanges() => AsNoTracking = false;

    }
}
