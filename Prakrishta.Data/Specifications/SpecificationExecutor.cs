namespace Prakrishta.Data.Specifications
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public static class SpecificationExecutor
    {
        public static IQueryable<T> EvaluateQuery<T>(IQueryable<T> inputQuery,
            ISpecification<T> spec) where T : class
        {
            IQueryable<T> query = inputQuery;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            foreach (var include in spec.IncludeProperties ?? [])
                query = query.Include(include);

            if (spec.PageNumber.HasValue && spec.PageSize.HasValue)
            {
                var skip = (spec.PageNumber.Value - 1) * spec.PageSize.Value;
                query = query.Skip(skip).Take(spec.PageSize.Value);
            }

            if(spec.OrderBy is not null)
                query = spec.OrderBy(query);

            if (spec.AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
