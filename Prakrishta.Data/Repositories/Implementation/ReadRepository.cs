//----------------------------------------------------------------------------------
// <copyright file="ReadRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>The read only repository implementation class</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Repositories.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Repositories.Interfaces;

    /// <summary>
    /// The repository class that performs all read operations
    /// </summary>
    /// <typeparam name="TEntity">The datatable entity type</typeparam>
    public class ReadRepository<TEntity> : RepositoryBase<TEntity>, IReadRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRepository.cs"/> class.
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public ReadRepository(DbContext dbContext) : base(dbContext)
        {

        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return this.GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking).ToList();
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return this.GetQueryable(null, orderBy, includeProperties, skip, take, asNoTracking).ToList();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.GetQueryable(null, orderBy, includeProperties, skip, take, asNoTracking)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null,
            int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        /// <inheritdoc />
        public async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default(CancellationToken))
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
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default(CancellationToken))
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
        public async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default(CancellationToken))
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
            CancellationToken cancellationToken = default(CancellationToken))
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
            bool asNoTracking = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.GetQueryable(filter, includeProperties: includeProperties, asNoTracking: asNoTracking)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
