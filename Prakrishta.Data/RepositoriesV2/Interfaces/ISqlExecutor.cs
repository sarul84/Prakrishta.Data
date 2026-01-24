//----------------------------------------------------------------------------------
// <copyright file="ISqlExecutor.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/22/2026</date>
// <summary>Contract that defines operations for Raw SQL</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISqlExecutor
    {
        /// <summary>
        /// Executes the specified SQL command asynchronously against the database.
        /// </summary>
        /// <remarks>The SQL command is executed directly against the database and is not processed by the
        /// Entity Framework change tracker. Use caution when executing raw SQL to avoid SQL injection
        /// vulnerabilities.</remarks>
        /// <param name="sql">The SQL command to execute. This can be a data definition or data manipulation statement.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected by
        /// the command.</returns>
        Task<int> ExecuteAsync(FormattableString sql, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the specified SQL command asynchronously against the database.
        /// </summary>
        /// <remarks>The SQL command is executed directly against the database and is not processed by the
        /// Entity Framework change tracker. Use caution when executing raw SQL to avoid SQL injection
        /// vulnerabilities.</remarks>
        /// <param name="sql">The SQL command to execute. This can be a data definition or data manipulation statement.</param>
        /// <param name="parameters">An object containing the parameters to be applied to the SQL command. Can be null if the command does not
        /// require parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected by
        /// the command.</returns>
        Task<int> ExecuteRawAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// Executes the specified SQL query asynchronously and maps the result set to a collection of entities of type
        /// TEntity.
        /// </summary>
        /// <remarks>The method executes the query in an asynchronous, non-blocking manner. The caller is
        /// responsible for ensuring that the SQL statement and parameters are valid and safe for execution. The
        /// returned collection may be empty if the query yields no results.</remarks>
        /// <typeparam name="TEntity">The type of the entities to which the query results are mapped. Must be a reference type.</typeparam>
        /// <param name="sql">The SQL query to execute. This string should be a valid SQL statement compatible with the underlying data
        /// source.</param>
        /// <param name="parameters">An object containing the parameters to be passed to the SQL query, or null if the query does not require
        /// parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// TEntity objects mapped from the query results.</returns>
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object? parameters = null, CancellationToken cancellationToken = default) where TEntity: class;

        /// <summary>
        /// Executes the specified SQL query asynchronously and returns a single result mapped to the specified entity
        /// type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to which the query result will be mapped. Must be a reference type.</typeparam>
        /// <param name="sql">The SQL query to execute. This should be a command that returns a single row.</param>
        /// <param name="parameters">An object containing the parameters to be passed to the SQL query, or null if no parameters are required.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped entity if a row is
        /// returned; otherwise, null.</returns>
        Task<TEntity?> QuerySingleAsync<TEntity>(string sql, object? parameters = null, CancellationToken cancellationToken = default) where TEntity : class;
    }
}
