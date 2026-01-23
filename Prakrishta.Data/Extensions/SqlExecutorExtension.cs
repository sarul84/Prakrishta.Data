//----------------------------------------------------------------------------------
// <copyright file="UnitOfWorkExtension.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/22/2026</date>
// <summary>Service collection extension to add SQL Executor Middleware</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Prakrishta.Data.RepositoriesV2.Implementations;
    using Prakrishta.Data.RepositoriesV2.Interfaces;

    /// <summary>
    /// Extension class to add SQL Executor middleware
    /// </summary>
    public static class SqlExecutorExtension
    {
        /// <summary>
        /// Adds the default implementation of ISqlExecutor to the service collection for dependency injection.
        /// </summary>
        /// <remarks>Registers EfCoreSqlExecutor as the scoped implementation for ISqlExecutor. Call this
        /// method during application startup to enable SQL execution services via dependency injection.</remarks>
        /// <param name="services">The IServiceCollection to which the ISqlExecutor service will be added.</param>
        /// <returns>The IServiceCollection instance with the ISqlExecutor service registered.</returns>
        public static IServiceCollection AddSqlExecutor(this IServiceCollection services)
        {
            services.AddScoped<ISqlExecutor, EfCoreSqlExecutor>();
            return services;
        }
    }
}
