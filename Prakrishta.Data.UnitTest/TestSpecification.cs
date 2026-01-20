using Prakrishta.Data.Specifications;
using System.Linq.Expressions;

namespace Prakrishta.Data.UnitTest
{
    public class TestSpecification : Specification<TestEntity>
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

}
