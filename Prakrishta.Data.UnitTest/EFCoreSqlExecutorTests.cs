using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.RepositoriesV2.Implementations;

namespace Prakrishta.Data.UnitTest
{
    public class EFCoreSqlExecutorTests
    {
        [Fact]
        public async Task QueryAsync_ReturnsAllRows()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            context.Users.AddRange(
                new User { Id = 1, Name = "A" },
                new User { Id = 2, Name = "B" }
            );
            context.SaveChanges();

            var executor = new EfCoreSqlExecutor(context);

            var result = await executor.QueryAsync<User>("SELECT * FROM Users");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task QueryAsync_FiltersRows()
        {
            using var context = SqliteInMemoryFactory.CreateContext();
            context.Users.AddRange(
                new User { Id = 1, Name = "John" },
                new User { Id = 2, Name = "Jane" }
            );

            context.SaveChanges();

            var executor = new EfCoreSqlExecutor(context);

            var result = await executor.QueryAsync<User>(
                "SELECT * FROM Users WHERE Name = 'John'"
            );

            Assert.Single(result);
            Assert.Equal("John", result.First().Name);
        }

        [Fact]
        public async Task QuerySingleAsync_ReturnsSingleRow()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            context.Users.Add(new User { Id = 1, Name = "John" });
            context.SaveChanges();

            var executor = new EfCoreSqlExecutor(context);

            var result = await executor.QuerySingleAsync<User>(
                "SELECT * FROM Users WHERE Id = 1"
            );

            Assert.NotNull(result);
            Assert.Equal("John", result!.Name);
        }

        [Fact]
        public async Task QuerySingleAsync_ReturnsNull_WhenNoMatch()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            context.Users.Add(new User { Id = 1, Name = "John" });
            context.SaveChanges();

            var executor = new EfCoreSqlExecutor(context);

            var result = await executor.QuerySingleAsync<User>(
                "SELECT * FROM Users WHERE Id = 99"
            );

            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_InsertsRow()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            var executor = new EfCoreSqlExecutor(context);

            var rows = await executor.ExecuteRawAsync(
                "INSERT INTO Users (Id, Name) VALUES (1, 'InsertedUser')"
            );

            Assert.Equal(1, rows);
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task ExecuteAsync_UpdatesRow()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            context.Users.Add(new User { Id = 1, Name = "Old" });
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var executor = new EfCoreSqlExecutor(context);

            var rows = await executor.ExecuteRawAsync(
                "UPDATE Users SET Name = 'New' WHERE Id = 1"
            );

            var newName = context.Users.First().Name;

            Assert.Equal(1, rows);
            Assert.Equal("New", newName);
        }

        [Fact]
        public async Task ExecuteAsync_DeletesRow()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            context.Users.Add(new User { Id = 1, Name = "ToDelete" });
            context.SaveChanges();

            var executor = new EfCoreSqlExecutor(context);

            var rows = await executor.ExecuteRawAsync(
                "DELETE FROM Users WHERE Id = 1"
            );

            Assert.Equal(1, rows);
            Assert.Empty(context.Users);
        }

        [Fact]
        public async Task QueryAsync_Throws_WhenTypeIsNotEntity()
        {
            using var context = SqliteInMemoryFactory.CreateContext();

            var executor = new EfCoreSqlExecutor(context);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await executor.QueryAsync<string>("SELECT 'Hello'");
            });
        }
    }
}
