//----------------------------------------------------------------------------------
// <copyright file="ICrudRepository.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>1/23/2019</date>
// <summary>Contract that defines CRUD operations</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Data.Repositories.Interfaces
{
    /// <summary>
    /// CRUD repository interface
    /// </summary>
    public interface ICrudRepository<TEntity> : ICudRepository<TEntity>, IReadRepository<TEntity> 
        where TEntity : class
    {
    }
}
