//----------------------------------------------------------------------------------
// <copyright file="AuditableEntity.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/24/2019</date>
// <summary>The Auditable Entity class</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Entities
{
    using Prakrishta.Data.Entities.Interfaces;
    using System;

    /// <summary>
    /// Entity class with auditable fields
    /// </summary>
    public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity where TId : struct
    {
        /// <summary>
        /// Gets or sets primary key or Id field
        /// </summary>
        object IAuditableEntity.Id { get { return this.Id; } set { } }

        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets Created Date
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Modified by
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
