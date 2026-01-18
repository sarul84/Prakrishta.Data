//----------------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>2/6/2019</date>
// <summary>RepositoryBase.cs</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="TEntity">The datatable entity type</typeparam>
    public class RepositoryBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets DB Context object
        /// </summary>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Gets DB Set (table) object
        /// </summary>
        protected DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase.cs"/> class.
        /// </summary>
        /// <param name="dbContext">The database context</param>
        protected RepositoryBase(DbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "The Database context is null");
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        /// <summary>
        /// Create a IQueryable object to query database context for the specified filter condition
        /// </summary>        
        ///  <param name="filter">The filter condition</param>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records required</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <returns>IQueryable entity</returns>
        protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? includeProperties = null, int? skip = null, int? take = null,
            bool asNoTracking = false)
        {
            includeProperties ??= string.Empty;
            IQueryable<TEntity> query = this.DbSet;

            foreach (var includeProperty in includeProperties.Split
                    ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (asNoTracking)
            {
                return query.AsNoTracking<TEntity>();
            }

            return query;
        }

        /// <summary>
        /// Creates an <see cref="IQueryable{TEntity}"/> that applies optional filtering, ordering, eager loading of
        /// related entities, paging, and tracking behavior to the underlying data set.
        /// </summary>
        /// <remarks>This method is typically used to build flexible queries against the underlying data
        /// set, allowing for composition of filters, ordering, eager loading, and paging in a single call. When
        /// <paramref name="asNoTracking"/> is set to <see langword="true"/>, the returned entities are not tracked by
        /// the context, which can improve performance for read-only scenarios.</remarks>
        /// <param name="filter">An expression used to filter the entities. Only entities matching the predicate will be included in the
        /// result. If null, no filtering is applied.</param>
        /// <param name="orderBy">A function to order the resulting entities. If null, the default ordering is used.</param>
        /// <param name="includeProperties">A list of expressions specifying related entities to include in the query results for eager loading. If null
        /// or empty, no related entities are included.</param>
        /// <param name="pageNumber">The page number for paginated results. Must be greater than 0 if specified. If null, no paging is applied.</param>
        /// <param name="pageSize">The number of entities to include in each page. Must be greater than 0 if specified. If null, no paging is
        /// applied.</param>
        /// <param name="asNoTracking">true to return entities without tracking changes in the context; otherwise, false.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> representing the query with the specified options applied. The query is
        /// not executed until enumerated.</returns>
        protected IQueryable<TEntity> EvaluateQuery(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            List<Expression<Func<TEntity, object>>>? includeProperties = null, int? pageNumber = null, int? pageSize = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = this.DbSet;

            foreach (var include in includeProperties ?? Enumerable.Empty<Expression<Func<TEntity, object>>>())
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            if (asNoTracking)
            {
                return query.AsNoTracking<TEntity>();
            }

            return query;
        }
    }
}
