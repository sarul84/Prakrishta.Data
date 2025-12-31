using System;

namespace Prakrishta.Data.Sample.DomainModel
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
