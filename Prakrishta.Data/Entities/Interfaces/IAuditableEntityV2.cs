//----------------------------------------------------------------------------------
// <copyright file="IAuditableEntity.cs" company="Prakrishta Technologies">
//     Copyright (c) 2025 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>12/15/2025</date>
// <summary>Contract that defines auditable entity</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Entities.Interfaces
{
    using System;

    public interface IAuditableEntityV2 : IAuditableEntity
    {
        /// <summary>
        /// 
        /// </summary>
        string DeletedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime DeletedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
