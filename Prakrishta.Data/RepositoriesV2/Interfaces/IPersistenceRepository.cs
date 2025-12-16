//----------------------------------------------------------------------------------
// <copyright file="IPersistenceRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>Contract that defines CUD operations</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for repository doing create, update and delete operations
    /// </summary>
    /// <typeparam name="TEnity">The datatable entity type</typeparam>
    /// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
    public interface IPersistenceRepository<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Add or create new entity to database set (table) and add it to underlying database through ORM tool
        /// </summary>
        /// <param name="entity">New entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds set of new entities to database set (table) and add it to underlying database through ORM tool
        /// </summary>
        /// <param name="entities">List of new entity</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        /// Add or create new entity to database set (table) and add it to underlying database through ORM tool
        /// </summary>
        /// <param name="entity">New entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns task that is awaitable</returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds set of new entities to database set (table) and add it to underlying database through ORM tool
        /// </summary>
        /// <param name="entities">List of new entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns task that is awaitable</returns>
        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update modified record into database table
        /// </summary>
        /// <param name="entity">The modified entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update list of modified record into database table
        /// </summary>
        /// <param name="entities">The modified entity list</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity from database table
        /// </summary>        
        /// <param name="id">Entity id that is going to be deleted</param>
        /// <param name="cancellationToken">Cancellation token</param>
        void Delete(TId id);

        /// <summary>
        /// Delete entity from database table
        /// </summary>
        /// <param name="entity">Entity that is going to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete list of entities from database table
        /// </summary>
        /// <param name="entities">Entities that are going to be deleted</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Marks the entity with the specified identifier as deleted without physically removing it from the data
        /// store.
        /// </summary>
        /// <remarks>Soft deletion typically flags the entity as deleted, allowing it to be excluded from
        /// standard queries while retaining the data for auditing or recovery purposes. The exact behavior may depend
        /// on the implementation.</remarks>
        /// <param name="id">The unique identifier of the entity to soft delete. Cannot be null.</param>
        void SoftDelete(TId id);

        /// <summary>
        /// Marks the specified entity as deleted without physically removing it from the data store.
        /// </summary>
        /// <remarks>Soft deletion typically sets a flag or status on the entity to indicate it is
        /// deleted, allowing it to be excluded from queries without being permanently removed. The exact behavior may
        /// depend on the implementation of the repository.</remarks>
        /// <param name="entity">The entity to be marked as deleted. Cannot be null.</param>
        void SoftDelete(TEntity entity);

        /// <summary>
        /// Marks the specified entities as deleted without physically removing them from the data store.
        /// </summary>
        /// <remarks>Soft deletion typically sets a flag or status on each entity to indicate it is
        /// deleted, allowing the data to be retained for auditing or recovery purposes. The entities will not be
        /// permanently removed from the underlying data store.</remarks>
        /// <param name="entities">The collection of entities to be marked as deleted. Cannot be null. Each entity in the collection will be
        /// soft deleted.</param>
        void SoftDelete(IEnumerable<TEntity> entities);
    }
}
