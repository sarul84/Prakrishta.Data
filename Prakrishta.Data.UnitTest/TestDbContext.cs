using Microsoft.EntityFrameworkCore;

namespace Prakrishta.Data.UnitTest
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();
        public DbSet<User> Users => Set<User>();
    }
}
