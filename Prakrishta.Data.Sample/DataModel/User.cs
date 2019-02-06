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
    using Prakrishta.Data.Entities;
    using System;

    /// <summary>
    /// User EF entity
    /// </summary>
    public class User : AuditableEntity<Guid>
    {
        /// <summary>
        /// Gets or sets User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets isactive flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets user first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user last name
        /// </summary>
        public string LastName { get; set; }
    }
}
