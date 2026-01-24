using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.RepositoriesV2.Implementations;
using Prakrishta.Data.Specifications;

namespace Prakrishta.Data.UnitTest
{
    public class SpecificationTests
    {
        private TestDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDbContext(options);
        }


        [Fact]
        public async Task GetAllAsync_UsesSpecification()
        {
            var context = CreateContext();

            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 });

            context.SaveChanges();

            var repo = new QueryRepository<TestEntity, int>(context);

            var spec = new TestSpecification(x => x.Id == 1);

            var result = await repo.GetAllAsync(spec);

            Assert.Single(result);
            Assert.Equal(1, result.First()?.Id);
        }

        [Fact]
        public void AddInclude_AddsIncludeExpression()
        {
            var spec = new TestSpecification(null);

            spec.AddIncludePublic(x => x.Child);

            Assert.Single(spec.IncludeProperties);
        }

        [Fact]
        public void ApplyPaging_SetsPagingProperties()
        {
            var spec = new TestSpecification(null);

            spec.ApplyPagingPublic(2, 10);

            Assert.True(spec.IsPagingEnabled);
            Assert.Equal(2, spec.PageNumber);
            Assert.Equal(10, spec.PageSize);
        }

        [Fact]
        public void ApplyOrderBy_SetsOrderByExpression()
        {
            var spec = new TestSpecification(null);

            spec.ApplyOrderByPublic(q => q.OrderBy(x => x.Id));

            Assert.NotNull(spec.OrderBy);
        }

        [Fact]
        public void TrackChanges_DisablesNoTracking()
        {
            var spec = new TestSpecification(null);

            spec.TrackChangesPublic();

            Assert.False(spec.AsNoTracking);
        }

        [Fact]
        public void EvaluateQuery_AppliesIncludes()
        {
            var context = CreateContext();

            context.TestEntities.Add(new TestEntity
            {
                Id = 1,
                Child = new TestChildEntity { Id = 10, Value = "Child" }
            });
            context.SaveChanges();

            var spec = new TestSpecification(null);
            spec.AddIncludePublic(x => x.Child);

            var query = SpecificationExecutor.EvaluateQuery(context.TestEntities, spec);

            var result = query.First();

            Assert.NotNull(result.Child);
            Assert.Equal(10, result.Child!.Id);
        }

        [Fact]
        public void EvaluateQuery_AppliesPaging()
        {
            var context = CreateContext();

            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 }
            );
            context.SaveChanges();

            var spec = new TestSpecification(null);
            spec.ApplyPagingPublic(2, 1); // page 2, size 1 → skip 1, take 1

            var query = SpecificationExecutor.EvaluateQuery(context.TestEntities, spec).ToList();

            Assert.Single(query);
            Assert.Equal(2, query[0].Id);
        }

        [Fact]
        public void EvaluateQuery_AppliesOrdering()
        {
            var context = CreateContext();

            context.TestEntities.AddRange(
                new TestEntity { Id = 3 },
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 }
            );
            context.SaveChanges();

            var spec = new TestSpecification(null);
            spec.ApplyOrderByPublic(q => q.OrderBy(x => x.Id));

            var result = SpecificationExecutor.EvaluateQuery(context.TestEntities, spec).ToList();

            Assert.Equal(new[] { 1, 2, 3 }, result.Select(x => x.Id).ToArray());
        }

        [Fact]
        public async Task EvaluateAsync_ReturnsCorrectCount_ForAggregationSpec()
        {
            // Arrange
            var context = CreateContext();

            context.TestEntities.AddRange(
                new TestEntity { Id = 1, IsDeleted = false },
                new TestEntity { Id = 2, IsDeleted = true },
                new TestEntity { Id = 3, IsDeleted = false});

            context.SaveChanges();

            var repo = new QueryRepository<TestEntity, int>(context);
            var spec = new ActiveTestCountSpec();

            // Act
            var result = await repo.EvaluateAsync(spec, CancellationToken.None);

            // Assert
            Assert.Equal(2, result);
        }

    }
}
