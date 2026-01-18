using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prakrishta.Data.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===============Connected Scenario=======");
            ConnectedEntityOperations();

            Console.WriteLine("===============DisConnected Scenario=======");
            DisconnectedEntityOperations();

            Console.ReadKey();
        }

        /// <summary>
        /// The below example shows disconnected scenario, typically web applications
        /// https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
        /// </summary>
        static void DisconnectedEntityOperations()
        {
            DatabaseContext databaseContext = new DatabaseContext();

            var domainUsers = new Collection<DomainModel.User>();

            using (var unitOfWork = new UnitOfWorkV2<DatabaseContext>(databaseContext))
            {
                var repository = unitOfWork.GetQueryRepository<User, Guid>();

                var users = repository?.GetAll() ?? [];

                foreach (var user in users)
                {
                    var domainUser = new DomainModel.User
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.Password,
                        IsActive = user.IsActive,
                        CreatedBy = user.CreatedBy,
                        CreatedOn = user.CreatedOn,
                        ModifiedBy = user.ModifiedBy,
                        ModifiedOn = user.ModifiedOn,
                        UserName = user.UserName
                    };
                    domainUsers.Add(domainUser);
                }

                repository = null;
                users = null;
            }

            Console.WriteLine("===============Current Records=======");
            foreach (var user in domainUsers)
            {
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedOn}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            Console.WriteLine("===============Changed Records=======");
            foreach (var user in domainUsers)
            {
                user.ModifiedBy = string.Empty;
                user.ModifiedOn = DateTime.UtcNow;
                Console.WriteLine($"User Full Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedOn}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            var dusers = new Collection<User>();
            foreach (var user in domainUsers)
            {
                var duser = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    IsActive = user.IsActive,
                    CreatedBy = user.CreatedBy,
                    CreatedOn = user.CreatedOn,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedOn = user.ModifiedOn,
                    UserName = user.UserName
                };
                dusers.Add(duser);
            }

            databaseContext = new DatabaseContext();
            using (var unitOfWork = new UnitOfWorkV2<DatabaseContext>(databaseContext))
            {
                unitOfWork.GetPersistenceRepository<User, Guid>()?.Update(dusers);
                unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                Console.WriteLine(Environment.NewLine + "Records updated successfully");
            }
        }

        /// <summary>
        /// The below example shows when business layer / UI layer(client) and DAL are part of the same exe,
        /// creating business object instance only once through IoC (Injecting repository or Unit of work in
        /// business class) and reusing the same object typically Desktop applications
        /// </summary>
        static void ConnectedEntityOperations()
        {
            DatabaseContext databaseContext = new DatabaseContext();

            var domainUsers = new Collection<DomainModel.User>();
            IUnitOfWorkV2<DatabaseContext> unitOfWork = new UnitOfWorkV2<DatabaseContext>(databaseContext);
            var repository = unitOfWork.GetQueryRepository<User,Guid>();

            var users = repository?.GetAll(asNoTracking: true) ?? [];

            foreach (var user in users)
            {
                var domainUser = new DomainModel.User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    IsActive = user.IsActive,
                    CreatedBy = user.CreatedBy,
                    CreatedOn = user.CreatedOn,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedOn = user.ModifiedOn,
                    UserName = user.UserName
                };
                domainUsers.Add(domainUser);
            }

            Console.WriteLine("===============Current Records=======");
            foreach (var user in domainUsers)
            {
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedOn}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            Console.WriteLine("===============Changed Records=======");
            foreach (var user in domainUsers)
            {
                user.ModifiedBy = "Disconnected";
                user.ModifiedOn = DateTime.UtcNow;
                Console.WriteLine($"User Full Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedOn}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            var dusers = new Collection<User>();
            foreach (var user in domainUsers)
            {
                var duser = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    IsActive = user.IsActive,
                    CreatedBy = user.CreatedBy,
                    CreatedOn = user.CreatedOn,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedOn = user.ModifiedOn,
                    UserName = user.UserName
                };
                dusers.Add(duser);
            }

            unitOfWork.GetPersistenceRepository<User, Guid>()?.Update(dusers);
            unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
            Console.WriteLine(Environment.NewLine + "Records updated successfully");
        }
    }
}
