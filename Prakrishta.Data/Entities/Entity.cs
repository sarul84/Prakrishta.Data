//----------------------------------------------------------------------------------
// <copyright file="Entity.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/24/2019</date>
// <summary>Entity base class</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Prakrishta.Data.Entities.Interfaces;

    /// <summary>
    ///  Abstract entity class
    /// </summary>
    /// <typeparam name="TId">The Id type</typeparam>
    public abstract class Entity<TId> : IEntity<TId> where TId : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TId Id { get; set; }
    }
}
