# Prakrishta.Data
Generic Repository and Unit of work pattern with EF.Core

Easy to create Unit of work and repository.

IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext);
var readRepository = unitOfWork.GetReadRepository<User>();

if CRUD repository is needed,
var readRepository = unitOfWork.GetCrudRepository<User>();
  
The unit of work can be injected using any IoC containers as well. There is extension to add Unit of work to .net core middleware.

services.AddUnitOfWork<urdbcontextclass>();

The code base is licensed under opern source, please feel free to use it in your project or edit as per your need.
