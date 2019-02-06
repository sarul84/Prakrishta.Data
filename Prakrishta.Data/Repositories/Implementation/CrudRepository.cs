//----------------------------------------------------------------------------------
// <copyright file="CrudRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>The CRUD repository implementation</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Repositories.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Repositories.Interfaces;

    /// <summary>
    /// The repository class that perform CRUD operation
    /// </summary>
    /// <typeparam name="TEntity">The datatable entity type</typeparam>
    public class CrudRepository<TEntity> : ReadRepository<TEntity>, IReadRepository<TEntity>, ICudRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudRepository.cs"/> class.
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public CrudRepository(DbContext dbContext) : base(dbContext)
        {

        }

        /// <inheritdoc />
        public virtual void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.DbSet.AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual void Add(IEnumerable<TEntity> entities)
        {
            this.DbSet.AddRange(entities);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.DbSet.AddRangeAsync(entities, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual void Delete(object id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = this.DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                this.DbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                TEntity entity = this.DbSet.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        /// <inheritdoc />
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            this.DbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual void Delete(TEntity entity)
        {
            if (this.DbContext.Entry(entity).State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
            this.DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            if (!this.DbContext.Entry(entity).IsKeySet)
            {
                throw new InvalidOperationException($"The primary key was not set on the entity class {entity.GetType().Name}");
            }

            this.DbSet.Update(entity);
        }

        /// <inheritdoc />
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            this.DbSet.UpdateRange(entities);
        }
    }
}
