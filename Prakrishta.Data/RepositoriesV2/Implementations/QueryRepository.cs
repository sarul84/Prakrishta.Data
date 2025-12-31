//----------------------------------------------------------------------------------
// <copyright file="QueryRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>The read only repository implementation class</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Entities.Interfaces;
    using Prakrishta.Data.Repositories;
    using Prakrishta.Data.RepositoriesV2.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The repository class that performs all read operations
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to be queried. Must implement the IAuditableBaseEntity<TId> interface.</typeparam>
    /// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
    /// <param name="dbContext"></param>
    public class QueryRepository<TEntity, TId>(DbContext dbContext) : RepositoryBase<TEntity>(dbContext)
        , IQueryRepository<TEntity, TId> where TEntity : class, IAuditableBaseEntity<TId>
    {
        /// <inheritdoc />
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return [.. this.GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking)];
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return [.. this.GetQueryable(null, orderBy, includeProperties, skip, take, asNoTracking)];
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(null, orderBy, includeProperties, skip, take, asNoTracking)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null,
            int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public TEntity GetById(TId id)
        {
            return this.DbSet.Find(id);
        }

        /// <inheritdoc />
        public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await this.DbSet
                .FindAsync(id, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        /// <inheritdoc />
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(filter)
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return this.GetQueryable(filter).Any();
        }

        /// <inheritdoc />
        public async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(filter)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, bool asNoTracking = false)
        {
            return this.GetQueryable(filter, orderBy, includeProperties, asNoTracking: asNoTracking)
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(filter, orderBy, includeProperties, asNoTracking: asNoTracking)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null,
            bool asNoTracking = false)
        {
            return this.GetQueryable(filter, includeProperties: includeProperties, asNoTracking: asNoTracking)
                .SingleOrDefault();
        }

        /// <inheritdoc />        
        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null,
            bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            return await this.GetQueryable(filter, includeProperties: includeProperties, asNoTracking: asNoTracking)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
