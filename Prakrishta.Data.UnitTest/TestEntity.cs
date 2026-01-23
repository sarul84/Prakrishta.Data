using Prakrishta.Data.Entities.Interfaces;

namespace Prakrishta.Data.UnitTest
{
    public class TestEntity : IAuditableBaseEntity<int>
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTimeOffset CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }

        // Navigation property for include tests
        public TestChildEntity? Child { get; set; }
    }

    public class TestChildEntity
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
