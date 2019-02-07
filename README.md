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
    var userList = this._uow.GetReadRepository<User>().GetAll();
}

```
The repository has both synchronous and asynchronous methods and also supports pagination as well. The associated tables can also queried through "Included" parameter, for example if user detail requires relevant entity "Address" as well then the following code snippet will do the job.

```
[HttpGet]
public ActionResult<IEnumerable<User>> Get()
{
    var userList = this._uow.GetReadRepository<User>().GetAll("Address");
}
```

All of the read methods support filtering and has support for sorting, record change tracking.

The code base is licensed under open source, please feel free to use it in your project or edit as per your need.
