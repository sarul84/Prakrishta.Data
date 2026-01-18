//----------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>2/6/2019</date>
// <summary>Class that implements IUnitOfWork contract</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data
{
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Entities.Interfaces;
    using Prakrishta.Data.Repositories.Implementation;
    using Prakrishta.Data.Repositories.Interfaces;
    using Prakrishta.Data.RepositoriesV2.Implementations;
    using Prakrishta.Data.RepositoriesV2.Interfaces;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Unit of work implementation
    /// </summary>
    /// <typeparam name="TContext">The database context</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UnitOfWork.cs"/> class.
    /// </remarks>
    /// <param name="context">The database context</param>
    [Obsolete("Use UnitOfWorkV2<TContext> instead.", error: false)]
    public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Holds collection of repositories
        /// </summary>
        private ConcurrentDictionary<string, object> repositories;

        /// <summary>
        /// Gets database context object
        /// </summary>
        public TContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context), "The database context is null");

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources database context.
        /// </summary>
        public void Dispose() => Context?.Dispose();

        /// <inheritdoc />
        public ICrudRepository<TEntity> GetCrudRepository<TEntity>() where TEntity : class
        {
            if (this.repositories == null)
            {
                this.repositories = new ConcurrentDictionary<string, object>();
            }

            var type = $"Crud - {typeof(TEntity).Name}";
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories.TryAdd(type, new CrudRepository<TEntity>(Context));
            }

            return this.repositories[type] as ICrudRepository<TEntity>;
        }

        /// <inheritdoc />
        public IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class
        {
            if (this.repositories == null)
            {
                this.repositories = new ConcurrentDictionary<string, object>();
            }

            var type = $"Read - {typeof(TEntity).Name}";
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories.TryAdd(type, new ReadRepository<TEntity>(Context));
            }

            return this.repositories[type] as IReadRepository<TEntity>;
        }

        /// <inheritdoc />
        public int SaveChanges()
        {
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    int updateCount = this.Context.SaveChanges();
                    transaction.Commit();
                    return updateCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    int updateCount = await this.Context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                    transaction.Commit();
                    return updateCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public class UnitOfWorkV2<TContext>(TContext context) : IUnitOfWorkV2<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Holds collection of repositories
        /// </summary>
        private ConcurrentDictionary<string, object> repositories;

        /// <summary>
        /// Gets database context object
        /// </summary>
        public TContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context), "The database context is null");

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources database context.
        /// </summary>
        public void Dispose() => Context?.Dispose();

        /// <inheritdoc />
        public IPersistenceRepository<TEntity, TId>? GetPersistenceRepository<TEntity, TId>() where TEntity : class, IAuditableBaseEntity<TId>
        {
            this.repositories ??= new ConcurrentDictionary<string, object>();

            var type = $"Persistence - {typeof(TEntity).Name}";
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories.TryAdd(type, new PersistenceRepository<TEntity, TId>(Context));
            }

            return this.repositories[type] as IPersistenceRepository<TEntity, TId>;
        }

        /// <inheritdoc />
        public IQueryRepository<TEntity, TId>? GetQueryRepository<TEntity, TId>() where TEntity : class, IAuditableBaseEntity<TId>
        {
            this.repositories ??= new ConcurrentDictionary<string, object>();

            var type = $"Query - {typeof(TEntity).Name}";
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories.TryAdd(type, new QueryRepository<TEntity, TId>(Context));
            }

            return this.repositories[type] as IQueryRepository<TEntity, TId>;
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    int updateCount = await this.Context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                    transaction.Commit();
                    return updateCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
