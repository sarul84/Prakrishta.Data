using Prakrishta.Data.Repositories.Implementation;
using System;
using System.Collections.ObjectModel;

namespace Prakrishta.Data.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseContext databaseContext = new DatabaseContext();

            var domainUsers = new Collection<DomainModel.User>();

            IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext);
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
                    ModifiedDate = user.ModifiedDate
                };
                domainUsers.Add(domainUser);
            }

            repository = null;
            users = null;

            Console.WriteLine("===============Current Records=======");
            foreach (var user in domainUsers)
            {
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Modified Date: {user.ModifiedDate}");                
            }

            //Console.WriteLine("===============Changed Records=======");
            //foreach (var user in domainUsers)
            //{
            //    user.ModifiedDate = DateTime.UtcNow;
            //    Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
            //    Console.WriteLine($"Modified Date: {user.ModifiedDate}");
            //}

            //Console.WriteLine("===============Map Domain / Data Model=======");
            //var dusers = new Collection<User>();
            //foreach (var user in domainUsers)
            //{
            //    var duser = new User
            //    {
            //        Id = user.Id,
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        Password = user.Password,
            //        IsActive = user.IsActive,
            //        CreatedBy = user.CreatedBy,
            //        CreatedDate = user.CreatedDate,
            //        ModifiedBy = user.ModifiedBy,
            //        ModifiedDate = user.ModifiedDate
            //    };
            //    dusers.Add(duser);
            //}
            
            Console.ReadKey();
        }
    }
}
