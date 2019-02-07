using System;
using System.Collections.ObjectModel;

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

            using (IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext))
            {
                var repository = unitOfWork.GetReadRepository<User>();

                var users = repository.GetAll();

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
                        CreatedDate = user.CreatedDate,
                        ModifiedBy = user.ModifiedBy,
                        ModifiedDate = user.ModifiedDate,
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
                Console.WriteLine($"Modified Date: {user.ModifiedDate}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            Console.WriteLine("===============Changed Records=======");
            foreach (var user in domainUsers)
            {
                user.ModifiedBy = string.Empty;
                user.ModifiedDate = DateTime.UtcNow;
                Console.WriteLine($"User Full Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedDate}");
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
                    CreatedDate = user.CreatedDate,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedDate = user.ModifiedDate,
                    UserName = user.UserName
                };
                dusers.Add(duser);
            }

            databaseContext = new DatabaseContext();
            using (IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext))
            {
                unitOfWork.GetCrudRepository<User>().Update(dusers);
                unitOfWork.SaveChanges();
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
            IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext);
            var repository = unitOfWork.GetReadRepository<User>();

            var users = repository.GetAll(asNoTracking: true);

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
                    CreatedDate = user.CreatedDate,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedDate = user.ModifiedDate,
                    UserName = user.UserName
                };
                domainUsers.Add(domainUser);
            }

            Console.WriteLine("===============Current Records=======");
            foreach (var user in domainUsers)
            {
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedDate}");
                Console.WriteLine($"Modified By: {user.ModifiedBy}");
            }

            Console.WriteLine("===============Changed Records=======");
            foreach (var user in domainUsers)
            {
                user.ModifiedBy = "Disconnected";
                user.ModifiedDate = DateTime.UtcNow;
                Console.WriteLine($"User Full Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedDate}");
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
                    CreatedDate = user.CreatedDate,
                    ModifiedBy = user.ModifiedBy,
                    ModifiedDate = user.ModifiedDate,
                    UserName = user.UserName
                };
                dusers.Add(duser);
            }

            unitOfWork.GetCrudRepository<User>().Update(dusers);
            unitOfWork.SaveChanges();
            Console.WriteLine(Environment.NewLine + "Records updated successfully");
        }
    }
}
