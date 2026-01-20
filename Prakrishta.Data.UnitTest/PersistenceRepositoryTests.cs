using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.RepositoriesV2.Implementations;

namespace Prakrishta.Data.UnitTest
{
    public class PersistenceRepositoryTests
    {
        private PersistenceRepository<TestEntity, int> CreateRepo(out TestDbContext context)
        {
            context = new TestDbContext(
                new DbContextOptionsBuilder<TestDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            return new PersistenceRepository<TestEntity, int>(context);
        }

        [Fact]
        public void Add_AddsEntity()
        {
            var repo = CreateRepo(out var context);

            repo.Add(new TestEntity { Id = 1 });
            context.SaveChanges();

            Assert.Equal(1, context.TestEntities.Count());
        }

        [Fact]
        public void Delete_RemovesEntity()
        {
            var repo = CreateRepo(out var context);

            context.TestEntities.Add(new TestEntity { Id = 2 });
            context.SaveChanges();
            context.ChangeTracker.Clear();

            repo.Delete(2);
            context.SaveChanges();

            Assert.Empty(context.TestEntities);
        }

        [Fact]
        public void SoftDelete_SetsFlags()
        {
            var repo = CreateRepo(out var context);

            var entity = new TestEntity { Id = 3 };
            context.TestEntities.Add(entity);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            repo.SoftDelete(entity);
            context.SaveChanges();

            Assert.True(entity.IsDeleted);
            Assert.NotNull(entity.DeletedOn);
        }

        [Fact]
        public void Update_UpdatesEntity()
        {
            var repo = CreateRepo(out var context);

            var entity = new TestEntity { Id = 1 };
            context.TestEntities.Add(entity);
            context.SaveChanges();

            entity.ModifiedBy = "test";
            repo.Update(entity);
            context.SaveChanges();

            Assert.Equal("test", context.TestEntities.First().ModifiedBy);
        }

        [Fact]
        public void Add_AddsSingleEntity()
        {
            var repo = CreateRepo(out var context);

            repo.Add(new TestEntity { Id = 1 });
            context.SaveChanges();

            Assert.Single(context.TestEntities);
        }

        [Fact]
        public void Add_AddsMultipleEntities()
        {
            var repo = CreateRepo(out var context);

            repo.Add(new[]
            {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 }
        });

            context.SaveChanges();

            Assert.Equal(2, context.TestEntities.Count());
        }

        [Fact]
        public async Task AddAsync_AddsSingleEntity()
        {
            var repo = CreateRepo(out var context);

            await repo.AddAsync(new TestEntity { Id = 1 });
            await context.SaveChangesAsync();

            Assert.Single(context.TestEntities);
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntities()
        {
            var repo = CreateRepo(out var context);

            await repo.AddAsync(new[]
            {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 }
        });

            await context.SaveChangesAsync();

            Assert.Equal(2, context.TestEntities.Count());
        }

        // ---------------------------
        // Delete(entity) / Delete(IEnumerable)
        // ---------------------------

        [Fact]
        public void Delete_RemovesSingleEntity()
        {
            var repo = CreateRepo(out var context);

            var entity = new TestEntity { Id = 1 };
            context.TestEntities.Add(entity);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            repo.Delete(entity);
            context.SaveChanges();

            Assert.Empty(context.TestEntities);
        }

        [Fact]
        public void Delete_RemovesMultipleEntities()
        {
            var repo = CreateRepo(out var context);

            var entities = new[]
            {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 }
        };

            context.TestEntities.AddRange(entities);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            repo.Delete(entities);
            context.SaveChanges();

            Assert.Empty(context.TestEntities);
        }

        // ---------------------------
        // Update(entity) / Update(IEnumerable)
        // ---------------------------

        [Fact]
        public void Update_UpdatesSingleEntity()
        {
            var repo = CreateRepo(out var context);

            var entity = new TestEntity { Id = 1, CreatedBy = "A" };
            context.TestEntities.Add(entity);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            entity.CreatedBy = "Updated";
            repo.Update(entity);
            context.SaveChanges();

            Assert.Equal("Updated", context.TestEntities.First().CreatedBy);
        }

        [Fact]
        public void Update_UpdatesMultipleEntities()
        {
            var repo = CreateRepo(out var context);

            var entities = new[]
            {
            new TestEntity { Id = 1, CreatedBy = "A" },
            new TestEntity { Id = 2, CreatedBy = "B" }
        };

            context.TestEntities.AddRange(entities);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            entities[0].CreatedBy = "Updated1";
            entities[1].CreatedBy = "Updated2";

            repo.Update(entities);
            context.SaveChanges();

            Assert.Contains(context.TestEntities, e => e.CreatedBy == "Updated1");
            Assert.Contains(context.TestEntities, e => e.CreatedBy == "Updated2");
        }

        // ---------------------------
        // SoftDelete(id) / SoftDelete(IEnumerable)
        // ---------------------------

        [Fact]
        public void SoftDelete_ById_SetsFlags()
        {
            var repo = CreateRepo(out var context);

            var entity = new TestEntity { Id = 1 };
            context.TestEntities.Add(entity);
            context.SaveChanges();

            repo.SoftDelete(1);
            context.SaveChanges();

            Assert.True(entity.IsDeleted);
            Assert.NotNull(entity.DeletedOn);
        }

        [Fact]
        public void SoftDelete_MultipleEntities_SetsFlags()
        {
            var repo = CreateRepo(out var context);

            var entities = new[]
            {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 }
        };

            context.TestEntities.AddRange(entities);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            repo.SoftDelete(entities);
            context.SaveChanges();

            Assert.All(entities, e => Assert.True(e.IsDeleted));
            Assert.All(entities, e => Assert.NotNull(e.DeletedOn));
        }

    }
}
