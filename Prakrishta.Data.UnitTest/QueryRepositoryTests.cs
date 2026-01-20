using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.RepositoriesV2.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prakrishta.Data.UnitTest
{
    public class QueryRepositoryTests
    {
        private QueryRepository<TestEntity, int> CreateRepo(out TestDbContext context)
        {
            context = new TestDbContext(
                new DbContextOptionsBuilder<TestDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            context.TestEntities.AddRange(
            new TestEntity { Id = 1, CreatedBy = "A" },
            new TestEntity { Id = 2, CreatedBy = "B" },
            new TestEntity { Id = 3, CreatedBy = "C" }
            );

            context.SaveChanges();

            context.SaveChanges();

            return new QueryRepository<TestEntity, int>(context);
        }

        [Fact]
        public void GetAll_ReturnsAll()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetAll();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetById_ReturnsEntity()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetExistsAsync_ReturnsTrue()
        {
            var repo = CreateRepo(out _);

            var exists = await repo.GetExistsAsync(x => x.Id == 1);

            Assert.True(exists);
        }

        [Fact]
        public void Get_ReturnsFilteredResults()
        {
            var repo = CreateRepo(out _);

            var result = repo.Get(x => x.Id > 1).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Get_AppliesOrderBy()
        {
            var repo = CreateRepo(out _);

            var result = repo.Get(orderBy: q => q.OrderByDescending(x => x.Id)).ToList();

            Assert.Equal(3, result.First().Id);
        }

        [Fact]
        public void Get_AppliesSkipAndTake()
        {
            var repo = CreateRepo(out _);

            var result = repo.Get(skip: 1, take: 1).ToList();

            Assert.Single(result);
            Assert.Equal(2, result[0].Id);
        }

        [Fact]
        public void Get_AsNoTracking_ReturnsDetachedEntities()
        {
            var repo = CreateRepo(out var context);

            var entity = repo.Get(asNoTracking: true).First();

            Assert.Equal(EntityState.Detached, context.Entry(entity).State);
        }

        // ---------------------------------------------------------
        // GetAllAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAllAsync();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_AppliesOrderBy()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAllAsync(orderBy: q => q.OrderBy(x => x.Id));

            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task GetAllAsync_AppliesSkipTake()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAllAsync(skip: 1, take: 1);

            Assert.Single(result);
            Assert.Equal(2, result.First().Id);
        }

        // ---------------------------------------------------------
        // GetAsync (async version of Get)
        // ---------------------------------------------------------

        [Fact]
        public async Task GetAsync_ReturnsFilteredResults()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAsync(x => x.Id == 1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAsync_AppliesOrderBy()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAsync(orderBy: q => q.OrderByDescending(x => x.Id));

            Assert.Equal(3, result.First().Id);
        }

        [Fact]
        public async Task GetAsync_AppliesSkipTake()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetAsync(skip: 2, take: 1);

            Assert.Single(result);
            Assert.Equal(3, result.First().Id);
        }

        // ---------------------------------------------------------
        // GetByIdAsync
        // ---------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetByIdAsync(999);

            Assert.Null(result);
        }

        // ---------------------------------------------------------
        // GetCount / GetCountAsync
        // ---------------------------------------------------------

        [Fact]
        public void GetCount_ReturnsCorrectCount()
        {
            var repo = CreateRepo(out _);

            var count = repo.GetCount(x => x.Id > 1);

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCorrectCount()
        {
            var repo = CreateRepo(out _);

            var count = await repo.GetCountAsync(x => x.Id <= 2);

            Assert.Equal(2, count);
        }

        // ---------------------------------------------------------
        // GetExists / GetExistsAsync
        // ---------------------------------------------------------

        [Fact]
        public void GetExists_ReturnsTrue()
        {
            var repo = CreateRepo(out _);

            Assert.True(repo.GetExists(x => x.Id == 1));
        }

        [Fact]
        public void GetExists_ReturnsFalse()
        {
            var repo = CreateRepo(out _);

            Assert.False(repo.GetExists(x => x.Id == 999));
        }

        // ---------------------------------------------------------
        // GetFirst (sync)
        // ---------------------------------------------------------

        [Fact]
        public void GetFirst_ReturnsFirstMatching()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetFirst(x => x.Id > 1);

            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void GetFirst_ReturnsOrderedFirst()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetFirst(orderBy: q => q.OrderByDescending(x => x.Id));

            Assert.Equal(3, result.Id);
        }

        // ---------------------------------------------------------
        // GetFirstAsync (specification)
        // ---------------------------------------------------------

        [Fact]
        public async Task GetFirstAsync_UsesSpecification()
        {
            var repo = CreateRepo(out _);

            var spec = new TestSpecification(x => x.Id == 2);

            var result = await repo.GetFirstAsync(spec, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        // ---------------------------------------------------------
        // GetOne / GetOneAsync
        // ---------------------------------------------------------

        [Fact]
        public void GetOne_ReturnsSingleOrNull()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetOne(x => x.Id == 1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetOne_ReturnsNull_WhenNoMatch()
        {
            var repo = CreateRepo(out _);

            var result = repo.GetOne(x => x.Id == 999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOneAsync_ReturnsSingle()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetOneAsync(x => x.Id == 3);

            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public async Task GetOneAsync_ReturnsNull_WhenNotFound()
        {
            var repo = CreateRepo(out _);

            var result = await repo.GetOneAsync(x => x.Id == 999);

            Assert.Null(result);
        }
    }
}
