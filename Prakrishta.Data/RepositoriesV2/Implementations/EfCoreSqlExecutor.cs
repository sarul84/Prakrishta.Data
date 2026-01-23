//----------------------------------------------------------------------------------
// <copyright file="EfCoreSqlExecutor.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/22/2026</date>
// <summary>Implementation of SQL Executor contract based for Entity framework</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.RepositoriesV2.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Prakrishta.Data.RepositoriesV2.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class EfCoreSqlExecutor(DbContext context) : ISqlExecutor
    {
        private readonly DbContext _context = context;

        </inheritdoc/>
        public async Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default)
            => await _context.Database.ExecuteSqlRawAsync(sql, ToDbParams(parameters));

        </inheritdoc/>
        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object? parameters = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => await _context.Set<TEntity>().FromSqlRaw(sql, ToDbParams(parameters)).ToListAsync(cancellationToken);

        </inheritdoc/>
        public async Task<TEntity?> QuerySingleAsync<TEntity>(string sql, object? parameters = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => await _context.Set<TEntity>().FromSqlRaw(sql, ToDbParams(parameters)).FirstOrDefaultAsync(cancellationToken);

        private static object[] ToDbParams(object? parameters)
            => parameters == null ? Array.Empty<object>() : new[] { parameters };
    }

}
