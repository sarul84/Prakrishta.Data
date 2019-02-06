//----------------------------------------------------------------------------------
// <copyright file="IAuditableEntity.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>Contract that defines auditable entity</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Entities.Interfaces
{
    using System;

    /// <summary>
    /// Interface that defines auditable entity
    /// </summary>
    public interface IAuditableEntity : IEntity
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        object Id { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets Created Date
        /// </summary>
        DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Modified by
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate
        /// </summary>
        DateTime? ModifiedDate { get; set; }
    }
}
