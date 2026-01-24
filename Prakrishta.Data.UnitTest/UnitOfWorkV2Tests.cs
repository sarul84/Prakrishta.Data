using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Prakrishta.Data.RepositoriesV2.Implementations;

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
        public void UnitOfWork_ExposesSqlExecutor()
        {
            using var context = SqliteInMemoryFactory.CreateContext();
            var sql = new EfCoreSqlExecutor(context);
            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            Assert.NotNull(uow.Sql);
        }

        [Fact]
        public async Task UnitOfWork_SqlAndRepositoryShareContext()
        {
            using var context = SqliteInMemoryFactory.CreateContext();
            var sql = new EfCoreSqlExecutor(context);
            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            var createdOn = DateTimeOffset.UtcNow;

            await uow.Sql.ExecuteAsync(
                $"INSERT INTO TestEntities (Id, CreatedBy, CreatedOn) VALUES ({1}, {"FromSql"}, {createdOn})"
            );

            var entity = await uow.GetQueryRepository<TestEntity, int>().GetByIdAsync(1);

            Assert.NotNull(entity);
            Assert.Equal("FromSql", entity!.CreatedBy);
            Assert.Equal(createdOn.ToUnixTimeSeconds(), entity.CreatedOn.ToUnixTimeSeconds());
        }

        [Fact]
        public async Task UnitOfWork_SqlParticipatesInTransaction()
        {
            using var context = SqliteInMemoryFactory.CreateContext();
            var sql = new EfCoreSqlExecutor(context);
            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            await uow.Sql.ExecuteAsync(
                $"INSERT INTO Users (Id, Name) VALUES ({1}, {"BeforeCommit"})"
            );

            await uow.SaveChangesAsync();

            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public void GetQueryRepository_ReturnsSameInstance()
        {
            var context = CreateContext();
            var sql = new EfCoreSqlExecutor(context);

            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            var repo1 = uow.GetQueryRepository<TestEntity, int>();
            var repo2 = uow.GetQueryRepository<TestEntity, int>();

            Assert.Same(repo1, repo2);
        }

        [Fact]
        public async Task SaveChangesAsync_CommitsTransaction()
        {
            var context = CreateContext();
            var sql = new EfCoreSqlExecutor(context);

            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            context.TestEntities.Add(new TestEntity { Id = 1 });

            var result = await uow.SaveChangesAsync();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetPersistenceRepository_ReturnsRepository()
        {
            var context = CreateContext();
            var sql = new EfCoreSqlExecutor(context);

            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            var repo = uow.GetPersistenceRepository<TestEntity, int>();

            Assert.NotNull(repo);
            Assert.IsType<PersistenceRepository<TestEntity, int>>(repo);
        }

        [Fact]
        public void GetPersistenceRepository_ReturnsSameInstanceOnSecondCall()
        {
            var context = CreateContext();
            var sql = new EfCoreSqlExecutor(context);

            var uow = new UnitOfWorkV2<TestDbContext>(context, sql);

            var repo1 = uow.GetPersistenceRepository<TestEntity, int>();
            var repo2 = uow.GetPersistenceRepository<TestEntity, int>();

            Assert.Same(repo1, repo2);
        }

    }
}
