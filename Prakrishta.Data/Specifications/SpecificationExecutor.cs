//----------------------------------------------------------------------------------
// <copyright file="SpecificationExecutor.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/19/2026</date>
// <summary>The static class defines Specification executor</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Specifications
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class SpecificationExecutor
    {
        /// <summary>
        /// Applies the specified query specification to the input query, including filtering, sorting, paging, and
        /// related entity inclusion.
        /// </summary>
        /// <remarks>This method does not execute the query; it returns a queryable object that
        /// incorporates the specification's criteria. If <paramref name="spec"/> requests no tracking, the returned
        /// query will not track changes in the context. Paging is applied only if both page number and page size are
        /// specified.</remarks>
        /// <typeparam name="T">The type of the elements in the query.</typeparam>
        /// <param name="inputQuery">The base query to which the specification will be applied.</param>
        /// <param name="spec">The specification that defines filtering criteria, sorting, paging, and related entity includes to apply to
        /// the query. Cannot be null.</param>
        /// <returns>An <see cref="IQueryable{T}"/> representing the input query with the specification applied. The returned
        /// query is not executed until enumerated.</returns>
        public static IQueryable<T> EvaluateQuery<T>(IQueryable<T> inputQuery,
            ISpecification<T> spec) where T : class
        {
            IQueryable<T> query = inputQuery;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            foreach (var include in spec.IncludeProperties ?? [])
                query = query.Include(include);

            if(spec.OrderBy is not null)
                query = spec.OrderBy(query);

            if (spec.PageNumber.HasValue && spec.PageSize.HasValue)
            {
                var skip = (spec.PageNumber.Value - 1) * spec.PageSize.Value;
                query = query.Skip(skip).Take(spec.PageSize.Value);
            }

            if (spec.AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        /// <summary>
        /// Executes the specified specification against the given queryable source and returns the aggregated or
        /// projected result asynchronously.
        /// </summary>
        /// <remarks>If the specification defines both an aggregation and a projection, the aggregation
        /// takes precedence. The method returns <see langword="null"/> if the projection yields no results.</remarks>
        /// <typeparam name="T">The type of the elements in the input query.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the specification.</typeparam>
        /// <param name="inputQuery">The queryable data source to evaluate the specification against.</param>
        /// <param name="spec">The specification that defines the aggregation or projection to apply to the input query. Must specify
        /// either an aggregation or a projection.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the aggregated or projected
        /// value, or <see langword="null"/> if no result is found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specification does not define either an aggregation or a projection.</exception>
        public static async Task<TResult?> EvaluateQuery<T, TResult>(
        IQueryable<T> inputQuery,
        ISpecification<T, TResult> spec,
        CancellationToken cancellationToken = default)
        where T : class
        {
            var query = EvaluateQuery(inputQuery, (ISpecification<T>)spec);

            // Aggregation takes priority
            if (spec.Aggregation is not null)
                return await spec.Aggregation(query);

            // Projection (Selector)
            if (spec.Projection is not null)
                return await spec.Projection(query).FirstOrDefaultAsync(cancellationToken);

            throw new InvalidOperationException(
                "Specification must define either Aggregation or Projection.");
        }

    }
}
