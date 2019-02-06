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
        public RepositoryBase(DbContext dbContext)
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
        protected virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null,
            bool asNoTracking = false)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = this.DbSet;

            try
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

                if (asNoTracking) query = query.AsNoTracking();
            }
            catch (Exception)
            {
                throw;
            }

            return query;
        }
    }
}
