//----------------------------------------------------------------------------------
// <copyright file="UnitOfWorkExtension.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>2/7/2019</date>
// <summary>Service collection extension to add unit of work to the middle ware</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension class to add unit of work middleware
    /// </summary>
    public static class UnitOfWorkExtension
    {
        /// <summary>
        /// Extension method to add unit of work to middleware
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        public static void AddUnitOfWork<TContext>(this IServiceCollection services) 
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        }
    }
}
