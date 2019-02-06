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
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Repositories.Implementation;
    using Prakrishta.Data.Repositories.Interfaces;

    /// <summary>
    /// Unit of work implementation
    /// </summary>
    /// <typeparam name="TContext">The database context</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Holds collection of repositories
        /// </summary>
        private IDictionary<Type, object> repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork.cs"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        public UnitOfWork(TContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets database context object
        /// </summary>
        public TContext Context { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources database context.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }

        /// <inheritdoc />
        public ICrudRepository<TEntity> GetCrudRepository<TEntity>() where TEntity : class
        {
            if (this.repositories == null)
            {
                this.repositories = new ConcurrentDictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories[type] = new CrudRepository<TEntity>(Context);
            }

            return (ICrudRepository<TEntity>)this.repositories[type];
        }

        /// <inheritdoc />
        public IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class
        {
            if (this.repositories == null)
            {
                this.repositories = new ConcurrentDictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!this.repositories.ContainsKey(type))
            {
                this.repositories[type] = new ReadRepository<TEntity>(Context);
            }

            return (IReadRepository<TEntity>)this.repositories[type];
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
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    int updateCount = await this.Context
                        .SaveChangesAsync()
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
