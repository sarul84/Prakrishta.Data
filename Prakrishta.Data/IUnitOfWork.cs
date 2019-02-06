//----------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>2/7/2019</date>
// <summary>Interface that defines Unit of work</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data
{
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.Repositories.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract that defines set of methods to be performed on repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///  Get readonly repository for the given database entity type
        /// </summary>
        /// <typeparam name="TEntity">Entity type for which repository required</typeparam>
        /// <returns>Readonly repository object</returns>
        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Get repository that would allow CRUD operations over the given database entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type for which repository required</typeparam>
        /// <returns>Repository object</returns>
        ICrudRepository<TEntity> GetCrudRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Save database context changes into database
        /// </summary>
        /// <returns>Affected records count</returns>
        int SaveChanges();

        /// <summary>
        /// Save database context changes into database
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Affected records count</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// Contract that defines set of methods to be performed on repositories
    /// </summary>
    /// <typeparam name="TContext">Database context type</typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Gets database context object
        /// </summary>
        TContext Context { get; }
    }
}
