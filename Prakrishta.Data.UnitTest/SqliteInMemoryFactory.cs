using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Prakrishta.Data.UnitTest
{
    public static class SqliteInMemoryFactory
    {
        public static TestDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new TestDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
