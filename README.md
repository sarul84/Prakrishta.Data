# Prakrishta.Data
Generic Repository and Unit of work pattern with EF.Core

Easy to create Unit of work and repository.

```
IUnitOfWork unitOfWork = new UnitOfWork<DatabaseContext>(databaseContext);
var readRepository = unitOfWork.GetReadRepository<User>();
```

if need to create CRUD repository,

```
var readRepository = unitOfWork.GetCrudRepository<User>();
```
  
The unit of work can be injected using any IoC containers as well. There is an extension to add Unit of work with .net core middleware.

```
services.AddUnitOfWork<urdbcontextclass>();
```
Once unit of work is added to middleware, can be injected to controller or whereever requires. The following example shows construction injection in controller and it's usage

```
private readonly IUnitOfWork<UserContext> _uow;
public UserController(IUnitOfWork<UserContext> uow)
{
    this._uow = uow;
}

[HttpGet]
public ActionResult<IEnumerable<User>> Get()
{
    var userRepository = this._uow.GetReadRepository<User>().GetAll();
}

```

The code base is licensed under opern source, please feel free to use it in your project or edit as per your need.
