//----------------------------------------------------------------------------------
// <copyright file="IAuditableEntityV3.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>Contract that defines auditable entity</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Entities.Interfaces
{
    using System;

    public interface IAuditableBaseEntity<TId>: IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        TId Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string? ModifiedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset? ModifiedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string? DeletedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset? DeletedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool? IsDeleted { get; set; }
    }
}
