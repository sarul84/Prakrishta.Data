using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Prakrishta.Data.RepositoriesV2.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prakrishta.Data.UnitTest
{
    public class UnitOfWorkV2Tests
    {
        private TestDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                .Options;

            return new TestDbContext(options);
        }

        [Fact]
        public void GetQueryRepository_ReturnsSameInstance()
        {
            var context = CreateContext();
            var uow = new UnitOfWorkV2<TestDbContext>(context);

            var repo1 = uow.GetQueryRepository<TestEntity, int>();
            var repo2 = uow.GetQueryRepository<TestEntity, int>();

            Assert.Same(repo1, repo2);
        }

        [Fact]
        public async Task SaveChangesAsync_CommitsTransaction()
        {
            var context = CreateContext();
            var uow = new UnitOfWorkV2<TestDbContext>(context);

            context.TestEntities.Add(new TestEntity { Id = 1 });

            var result = await uow.SaveChangesAsync();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetPersistenceRepository_ReturnsRepository()
        {
            var context = CreateContext();
            var uow = new UnitOfWorkV2<TestDbContext>(context);

            var repo = uow.GetPersistenceRepository<TestEntity, int>();

            Assert.NotNull(repo);
            Assert.IsType<PersistenceRepository<TestEntity, int>>(repo);
        }

        [Fact]
        public void GetPersistenceRepository_ReturnsSameInstanceOnSecondCall()
        {
            var context = CreateContext();
            var uow = new UnitOfWorkV2<TestDbContext>(context);

            var repo1 = uow.GetPersistenceRepository<TestEntity, int>();
            var repo2 = uow.GetPersistenceRepository<TestEntity, int>();

            Assert.Same(repo1, repo2);
        }

    }
}
