//----------------------------------------------------------------------------------
// <copyright file="User.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>User Entity</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Sample
{
    using Prakrishta.Data.Entities.Interfaces;
    using System;

    /// <summary>
    /// User EF entity
    /// </summary>
    public class User : IAuditableBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets User Name
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets isactive flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets user first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user last name
        /// </summary>
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
