using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Prakrishta.Data.UnitTest
{
    public class RepositoryBaseTests
    {
        private TestDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDbContext(options);
        }

        private class TestRepository : RepositoryBase<TestEntity>
        {
            public TestRepository(DbContext context) : base(context) { }

            public IQueryable<TestEntity> InvokeQueryable(
                Expression<Func<TestEntity, bool>>? filter = null,
                Func<IQueryable<TestEntity>, IOrderedQueryable<TestEntity>>? orderBy = null,
                string? includeProperties = null,
                int? skip = null,
                int? take = null,
                bool asNoTracking = false)
            {
                return GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking);
            }
        }

        [Fact]
        public void Constructor_Throws_WhenDbContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestRepository(null!));
        }

        [Fact]
        public void GetQueryable_ReturnsAll_WhenNoParametersProvided()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable().ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetQueryable_AppliesFilter()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(x => x.Id == 1).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public void GetQueryable_AppliesOrderBy()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 2 },
                new TestEntity { Id = 1 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(
                orderBy: q => q.OrderBy(x => x.Id)).ToList();

            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public void GetQueryable_AppliesSkip()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(skip: 1).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].Id);
        }

        [Fact]
        public void GetQueryable_AppliesTake()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(take: 2).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetQueryable_AppliesSkipAndTakeTogether()
        {
            var context = CreateContext();
            context.TestEntities.AddRange(
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(skip: 1, take: 1).ToList();

            Assert.Single(result);
            Assert.Equal(2, result[0].Id);
        }

        [Fact]
        public void GetQueryable_AsNoTracking_ReturnsUntrackedEntities()
        {
            var context = CreateContext();
            context.TestEntities.Add(new TestEntity { Id = 1 });
            context.SaveChanges();

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(asNoTracking: true).First();

            var entry = context.Entry(result);

            Assert.Equal(EntityState.Detached, entry.State);
        }

        [Fact]
        public void GetQueryable_IncludeProperties_ParsesMultipleIncludes()
        {
            var context = CreateContext();

            // Add navigation properties to TestEntity if needed
            // For now, we only verify that Include() is called without throwing

            var repo = new TestRepository(context);

            var result = repo.InvokeQueryable(includeProperties: "PropA,PropB");

            Assert.NotNull(result);
        }
    }
}
