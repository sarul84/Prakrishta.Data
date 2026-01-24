//----------------------------------------------------------------------------------
// <copyright file="IReadRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>Contract that defines read only operations</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Interfaces
{
    using Prakrishta.Data.Entities.Interfaces;
    using Prakrishta.Data.Specifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for querying entities from a data source using flexible filtering, ordering, and projection
    /// options.
    /// </summary>
    /// <remarks>This interface provides a set of methods for retrieving entities with support for synchronous
    /// and asynchronous operations, filtering, sorting, pagination, and eager loading of related data. Implementations
    /// are typically used to abstract data access logic and enable testability and separation of concerns in
    /// application architecture.</remarks>
    /// <typeparam name="TEntity">The type of the entity to be queried. Must implement the IAuditableBaseEntity<TId> interface.</typeparam>
    /// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
    public interface IQueryRepository<TEntity, TId> where TEntity : class, IAuditableBaseEntity<TId>
    {
        /// <summary>
        /// Retrieves all entities of type TEntity from the data source, with optional ordering, filtering, and
        /// pagination.
        /// </summary>
        /// <remarks>When asNoTracking is set to true, the returned entities are not tracked by the
        /// context, which is recommended for read-only operations. The includeProperties parameter allows eager loading
        /// of related data by specifying navigation property names separated by commas.</remarks>
        /// <param name="orderBy">A function to apply ordering to the query. If null, the default order is used.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results. Use to specify
        /// navigation properties for eager loading. If null or empty, no related entities are included.</param>
        /// <param name="skip">The number of entities to skip before starting to return results. If null, no entities are skipped.</param>
        /// <param name="take">The maximum number of entities to return. If null, all remaining entities are returned.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entities, which can improve performance for read-only
        /// scenarios; otherwise, false.</param>
        /// <returns>An enumerable collection of TEntity objects that match the specified criteria. The collection may be empty
        /// if no entities are found.</returns>
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string? includeProperties = null, int? skip = null,
            int? take = null, bool asNoTracking = false);

        /// <summary>
        /// Asynchronously retrieves all entities, with optional ordering, related property inclusion, paging, and
        /// tracking behavior.
        /// </summary>
        /// <remarks>This method is typically used to retrieve multiple entities from a data source with
        /// flexible query options. When asNoTracking is set to true, the returned entities are not tracked by the
        /// context, which is recommended for read-only scenarios.</remarks>
        /// <param name="orderBy">A function to apply ordering to the query. If null, no specific ordering is applied.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results. Use to eagerly load
        /// related data. If null or empty, no related entities are included.</param>
        /// <param name="skip">The number of entities to skip before starting to return results. If null, no entities are skipped.</param>
        /// <param name="take">The maximum number of entities to return. If null, all remaining entities are returned.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entities, which can improve performance for read-only
        /// operations; otherwise, false.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// entities matching the specified criteria. The collection will be empty if no entities are found.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string? includeProperties = null,
            int? skip = null, int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of entities that match the specified criteria, with optional filtering, sorting,
        /// eager loading, paging, and tracking behavior.
        /// </summary>
        /// <remarks>This method supports advanced querying scenarios by allowing filtering, sorting,
        /// eager loading of related data, and paging. When asNoTracking is set to true, the returned entities are not
        /// tracked by the underlying context, which is recommended for read-only queries to improve
        /// performance.</remarks>
        /// <param name="filter">An expression used to filter the entities to be returned. If null, no filtering is applied.</param>
        /// <param name="orderBy">A function to order the resulting entities. If null, the default ordering is used.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results for eager loading.
        /// If null or empty, no related entities are included.</param>
        /// <param name="skip">The number of entities to skip before starting to return results. If null, no entities are skipped.</param>
        /// <param name="take">The maximum number of entities to return. If null, all remaining entities are returned.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entities; otherwise, false. Disabling tracking can improve
        /// performance for read-only operations.</param>
        /// <returns>An enumerable collection of entities that satisfy the specified criteria. The collection may be empty if no
        /// entities match.</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false);

        /// <summary>
        /// Asynchronously retrieves a collection of entities that match the specified criteria, with optional
        /// filtering, ordering, related data inclusion, paging, and tracking behavior.
        /// </summary>
        /// <remarks>This method is typically used to query entities from a data source with flexible
        /// filtering, sorting, and eager loading options. When asNoTracking is set to true, the returned entities are
        /// not tracked by the context, which is recommended for read-only scenarios.</remarks>
        /// <param name="filter">An expression used to filter the entities to be returned. If null, all entities are included.</param>
        /// <param name="orderBy">A function to order the resulting entities. If null, the default ordering is applied.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results. Use to eagerly load
        /// related data. If null or empty, no related entities are included.</param>
        /// <param name="skip">The number of entities to skip before starting to collect the result set. If null, no entities are skipped.</param>
        /// <param name="take">The maximum number of entities to return. If null, all remaining entities are returned.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entities; otherwise, false. Disabling tracking can improve
        /// performance for read-only operations.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// entities that match the specified criteria. The collection is empty if no entities are found.</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single entity that matches the specified filter criteria from the data source.
        /// </summary>
        /// <remarks>If multiple entities match the filter, only the first one is returned. When
        /// asNoTracking is set to true, the returned entity is not tracked by the context, which is recommended for
        /// read-only scenarios.</remarks>
        /// <param name="filter">An expression used to filter the entities to retrieve. If null, no filtering is applied and the first entity
        /// is returned.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include in the query results. Specify property names to
        /// eagerly load related data. If null or empty, no related entities are included.</param>
        /// <param name="asNoTracking">true to return the entity without tracking it in the context; otherwise, false. Use true for read-only
        /// operations to improve performance.</param>
        /// <returns>The first entity that matches the filter criteria, or null if no such entity is found.</returns>
        TEntity? GetOne(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null, bool asNoTracking = false);

        /// <summary>
        /// Asynchronously retrieves a single entity that matches the specified filter criteria.
        /// </summary>
        /// <remarks>If multiple entities match the filter, only the first one is returned. This method is
        /// typically used to retrieve a single entity by a unique property or identifier.</remarks>
        /// <param name="filter">An expression used to filter the entities. If null, no filtering is applied and the first entity is
        /// returned.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include in the query results. Specify property names to
        /// eagerly load related data. If null or empty, no related entities are included.</param>
        /// <param name="asNoTracking">true to return the entity without tracking it in the context; otherwise, false. Use true for read-only
        /// operations to improve performance.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches
        /// the filter, or null if no entity is found.</returns>
        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null,
            bool asNoTracking = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the first entity that matches the specified filter, with optional ordering, related data
        /// inclusion, and tracking behavior.
        /// </summary>
        /// <remarks>If multiple entities match the filter, the first one according to the specified
        /// ordering is returned. When asNoTracking is set to true, the returned entity is not tracked by the context,
        /// which is recommended for read-only scenarios.</remarks>
        /// <param name="filter">An expression used to filter the entities to be considered. If null, all entities are considered.</param>
        /// <param name="orderBy">A function to order the filtered entities. If null, the default ordering is used.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results. If null or empty,
        /// no related entities are included.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entity; otherwise, false. Disabling tracking can improve
        /// performance for read-only operations.</param>
        /// <returns>The first entity that matches the specified criteria, or null if no such entity is found.</returns>
        TEntity? GetFirst(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? includeProperties = null, bool asNoTracking = false);

        /// <summary>
        /// Asynchronously returns the first entity that matches the specified filter, or the first entity in the
        /// sequence if no filter is provided.
        /// </summary>
        /// <param name="filter">An expression used to filter the entities to search. If null, all entities are considered.</param>
        /// <param name="orderBy">A function to order the resulting entities. If null, the default ordering is used.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query results. If null or empty,
        /// no related entities are included.</param>
        /// <param name="asNoTracking">true to disable change tracking for the returned entity; otherwise, false. Disabling tracking can improve
        /// performance for read-only operations.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches
        /// the filter, or null if no such entity is found.</returns>
        Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? includeProperties = null, bool asNoTracking = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null.</param>
        /// <returns>The entity that matches the specified identifier, or null if no such entity exists.</returns>
        TEntity? GetById(TId id);

        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise,
        /// null.</returns>
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the number of entities that satisfy the specified filter condition.
        /// </summary>
        /// <param name="filter">An expression that defines the filter to apply to the entities. If null, the method returns the total count
        /// of all entities.</param>
        /// <returns>The number of entities that match the filter condition. Returns the total number of entities if <paramref
        /// name="filter"/> is null.</returns>
        int GetCount(Expression<Func<TEntity, bool>>? filter = null);

        /// <summary>
        /// Asynchronously returns the number of entities that satisfy the specified filter condition.
        /// </summary>
        /// <param name="filter">An expression used to filter the entities to be counted. If null, all entities are counted.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities that
        /// match the filter condition.</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether any entities exist in the data source that match the specified filter.
        /// </summary>
        /// <param name="filter">An expression used to filter the entities to check for existence. If null, the method checks for the
        /// existence of any entities.</param>
        /// <returns>true if at least one entity matches the filter; otherwise, false.</returns>
        bool GetExists(Expression<Func<TEntity, bool>>? filter = null);

        /// <summary>
        /// Asynchronously determines whether any entities exist that match the specified filter criteria.
        /// </summary>
        /// <param name="filter">An expression used to filter the entities to check for existence. If null, the method checks for the
        /// existence of any entities.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if at
        /// least one entity matches the filter; otherwise, <see langword="false"/>.</returns>
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all entities that satisfy the specified criteria.
        /// </summary>
        /// <param name="specification">The specification that defines the criteria used to filter the entities to retrieve. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities
        /// that match the specification. The list will be empty if no entities are found.</returns>
        Task<IReadOnlyList<TEntity?>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified criteria.
        /// </summary>
        /// <param name="specification">The specification that defines the criteria used to filter entities. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches
        /// the specification, or null if no such entity is found.</returns>
        Task<TEntity?> GetFirstAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously evaluates the specified specification against the current entity and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the specification evaluation.</typeparam>
        /// <param name="specification">The specification to evaluate. Defines the criteria and projection for the evaluation. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the value produced by evaluating
        /// the specification.</returns>
        Task<TResult?> EvaluateAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken1);
    }
}
