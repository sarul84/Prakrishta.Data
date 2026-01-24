//----------------------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="Prakrishta Technologies">
//     Copyright (c) 2026 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>01/19/2026</date>
// <summary>The interface which defines the Specification contract</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets or sets the Criteria.
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Gets or sets the Order By.
        /// </summary>
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }

        /// <summary>
        /// Gets or sets the Include Properties.
        /// </summary>
        IList<Expression<Func<T, object>>>? IncludeProperties { get; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        int? PageNumber { get; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        int? PageSize { get; }

        // <summary>
        /// Gets or sets Is Paging Enable flag.
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Gets or sets the AsNoTracking flag.
        /// </summary>
        bool AsNoTracking { get; }
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        /// <summary>
        /// Gets or sets the Selector.
        /// </summary>
        Func<IQueryable<T>, IQueryable<TResult>>? Projection { get; }

        /// <summary>
        /// Gets or sets the Aggregation.
        /// </summary>
        Func<IQueryable<T>, Task<TResult>>? Aggregation { get; }
    }
}
