using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.Specifications;
using System.Linq.Expressions;

namespace Prakrishta.Data.UnitTest
{
    public sealed class TestSpecification : Specification<TestEntity>
    {
        public TestSpecification(Expression<Func<TestEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }

        public void AddIncludePublic(Expression<Func<TestEntity, object>> include)
            => AddInclude(include);

        public void ApplyPagingPublic(int page, int size)
            => ApplyPaging(page, size);

        public void ApplyOrderByPublic(Func<IQueryable<TestEntity>, IOrderedQueryable<TestEntity>> orderBy)
            => ApplyOrderBy(orderBy);

        public void TrackChangesPublic()
            => TrackChanges();
    }

    public sealed class ActiveTestCountSpec : ISpecification<TestEntity, int>
    {
        public Expression<Func<TestEntity, bool>>? Criteria => u => u.IsDeleted == false;
        public IList<Expression<Func<TestEntity, object>>>? IncludeProperties => null;
        public Func<IQueryable<TestEntity>, IOrderedQueryable<TestEntity>>? OrderBy => null;
        public int? PageNumber => null;
        public int? PageSize => null;
        public bool AsNoTracking => true;

        public bool IsPagingEnabled => false;

        public Func<IQueryable<TestEntity>, IQueryable<int>>? Projection => null;

        public Func<IQueryable<TestEntity>, Task<int>> Aggregation =>
            q => q.CountAsync();
    }


}
