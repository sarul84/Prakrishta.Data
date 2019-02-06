//----------------------------------------------------------------------------------
// <copyright file="IReadRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>Contract that defines read operation</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository interface with READ only operations
    /// </summary>
    /// <typeparam name="TEntity">The datatable entity type</typeparam>
    public interface IReadRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all the records from the database table
        /// </summary>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records required</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <returns>The list of entities</returns>
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null,
            int? take = null, bool asNoTracking = false);

        /// <summary>
        /// Asyncronously get all the records from the database table
        /// </summary>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records required</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The list of entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null,
            int? skip = null, int? take = null, bool asNoTracking = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get all records from the database table for the given filter condition
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records required</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <returns>The list of entities from the table</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false);

        /// <summary>
        /// Get all records from the database table for the given filter condition (Async)
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="skip">The number of records to be skipped</param>
        /// <param name="take">The number of records required</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The list of entities from the table</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null, bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a record from the database table for the given filter condition
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <returns>The entity from the table</returns>
        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null, bool asNoTracking = false);

        /// <summary>
        /// Get a record from the database table for the given filter condition (async)
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entity from the table</returns>
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null, 
            bool asNoTracking = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a first record from the database table for the given filter condition
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="asNoTracking">True if table tracking required</param>        
        /// <returns>The entity from the table</returns>
        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, bool asNoTracking = false);

        /// <summary>
        /// Get a first record from the database table for the given filter condition (async)
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="orderBy">The order by condition</param>
        /// <param name="includeProperties">The properties to be included in the select query</param>
        /// <param name="asNoTracking">True if table tracking required</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entity from the table</returns>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, bool asNoTracking = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a record from the database table for the specific id
        /// </summary>
        /// <param name="id">The primary key or id to filter a record</param>
        /// <returns>The entity from the table</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Get a record from the database table for the specific id (async)
        /// </summary>
        /// <param name="id">The primary key or id to filter a record</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entity from the table</returns>
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the count of records for the specified filter condition
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <returns>Number of records</returns>
        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Get the count of records for the specified filter condition (async)
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of records</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check if the record exists in database
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <returns>Returns TRUE if record exists otherwise FALSE</returns>
        bool GetExists(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Check if the record exists in database (async)
        /// </summary>
        /// <param name="filter">The filter condition</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns TRUE if record exists otherwise FALSE</returns>
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
