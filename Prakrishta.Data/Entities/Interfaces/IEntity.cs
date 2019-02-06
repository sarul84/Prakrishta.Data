//------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/22/2019</date>
// <summary>Contract that defines basic EF entity</summary>
//--------------------------------------------------------------------------

namespace Prakrishta.Data.Entities.Interfaces
{
    /// <summary>
    /// Generic interface for basic EF entity with Id field
    /// </summary>
    /// <typeparam name="TId">Id data type</typeparam>
    public interface IEntity<TId> : IEntity where TId : struct
    {
        /// <summary>
        /// Gets or sets Id (Primary key) field
        /// </summary>
        TId Id { get; set; }
    }

    /// <summary>
    /// Marker interface for basic EF entity without id field
    /// </summary>
    public interface IEntity { }
}
