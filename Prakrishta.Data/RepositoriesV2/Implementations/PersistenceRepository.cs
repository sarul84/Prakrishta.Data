//----------------------------------------------------------------------------------
// <copyright file="PersistenceRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>The CRUD repository implementation</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Entities.Interfaces;
    using Prakrishta.Data.RepositoriesV2.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class PersistenceRepository<TEntity, TId>(DbContext dbContext) : QueryRepository<TEntity, TId>(dbContext)
        , IPersistenceRepository<TEntity, TId> where TEntity : class, IAuditableBaseEntity<TId>
    {
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
        public virtual void Delete(TId id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var entityType = DbContext.Model.FindEntityType(typeInfo);
            if (entityType != null)
            {
                // CA1826: Do not use Enumerable methods on indexable collections. Instead use the collection directly.
                // entityType.FindPrimaryKey().Properties is IReadOnlyList<IProperty>, so use [0] instead of FirstOrDefault()
                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey != null && primaryKey.Properties.Any())
                {
                    var key = primaryKey.Properties[0];
                    var property = typeInfo.GetProperty(key.Name);

                    if (property != null)
                    {
                        var entity = Activator.CreateInstance<TEntity>();
                        property.SetValue(entity, id);
                        DbContext.Entry(entity).State = EntityState.Deleted;
                        return;
                    }
                }
            }

            var entityFound = DbSet.Find(id);
            if (entityFound != null)
            {
                Delete(entityFound);
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

        /// <inheritdoc />
        public void SoftDelete(TId id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
            {
                SoftDelete(entity);
            }
        }

        /// <inheritdoc />
        public void SoftDelete(TEntity entity)
        {

            SetSoftDeleteProperties(entity);
            DbSet.Update(entity);
        }

        /// <inheritdoc />
        public void SoftDelete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetSoftDeleteProperties(entity);
            }

            Update(entities);
        }

        private void SetSoftDeleteProperties(TEntity entity)
        {
            var type = typeof(TEntity);

            var isDeletedProp = type.GetProperty("IsDeleted");
            if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
            {
                isDeletedProp.SetValue(entity, true);
            }

            var deletedDateProp = type.GetProperty("DeletedDate");
            if (deletedDateProp != null && (deletedDateProp.PropertyType == typeof(DateTime?)))
            {
                deletedDateProp.SetValue(entity, DateTime.UtcNow);
            }
            else
            {
                var deletedOnProp = type.GetProperty("DeletedOn");
                if (deletedOnProp != null && (deletedOnProp.PropertyType == typeof(DateTimeOffset?)))
                {
                    deletedOnProp.SetValue(entity, DateTimeOffset.UtcNow);
                }
            }
        }
    }
}
