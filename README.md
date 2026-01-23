# Prakrishta.Data
## Overview

**Prakrishta.Data** is a lightweight, extensible data access library for Entity Framework Core that provides:
- **Unit of Work** pattern for transactional consistency.
- **Generic Query** Repository for read-only operations.
- **Generic Persistence Repository** for full CRUD operations.
- **Specification Pattern** for reusable, composable query logic.
- **Raw SQL Execution** for safe and flexible abstraction for executing raw SQL queries when LINQ or the specification pattern is not sufficient.
    - Reporting and analytics queries
    - Complex joins or projections
    - Stored procedures
    - Bulk operations
    - Performance‑critical paths
- **Support for**:
    - Synchronous and asynchronous methods.
    - Pagination.
    - Sorting.
    - Change tracking control.
    - Eager loading via Include expressions or string paths.
- First-class DI integration with .NET Core.


Build status:
[![Build Status](https://github.com/sarul84/Prakrishta.Data/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sarul84/Prakrishta.Data/actions/workflows/dotnet.yml)

Package version: ![NuGet](https://img.shields.io/nuget/v/Prakrishta.Data)

Package installation:

```
Install-Package Prakrishta.Data
```

## Core concepts

**Unit of work**

The **Unit of Work** coordinates repositories and manages the underlying DbContext lifetime and transactions.

```
IUnitOfWorkV2<DatabaseContext> unitOfWork = new UnitOfWorkV2<DatabaseContext>(databaseContext);
var readRepository = unitOfWork.GetQueryRepository<User>();
```

From the unit of work, you can obtain:

- Query repository (read-only):

```
var readRepository = unitOfWork.GetQueryRepository<User>();
```

- Persistence repository (CRUD):

```
var readRepository = unitOfWork.GetPersistenceRepository<User>();
```

Generic overloads with key type are also supported:

```
var readRepository = unitOfWork.GetQueryRepository<User, Guid>();
var persistenceRepository = unitOfWork.GetPersistenceRepository<User, Guid>();
```

## Repository types

**Query repository**

The **query repository** is optimized for read operations:
- Get all entities.
- Filtered queries.
- Sorting.
- Pagination.
- Eager loading of related entities.
- Optional change tracking.
  
Example:

```
var users = _uow
    .GetQueryRepository<User, Guid>()
    .GetAll();
```

With includes:

```
var usersWithAddress = _uow
    .GetQueryRepository<User, Guid>()
    .GetAll("Address");
```

**Persistence repository**

The **persistence repository** supports full CRUD:
- Add / AddRange
- Update / UpdateRange
- Delete / DeleteRange

Example:

```
var repo = _uow.GetPersistenceRepository<User, Guid>();

repo.Add(newUser);
_uow.SaveChanges();
```

Async:
```
await repo.AddAsync(newUser);
await _uow.SaveChangesAsync();
```

**Dependency injection and middleware integration**

You can register the unit of work in your .NET Core application using the provided extension:

```
services.AddUnitOfWork<UserContext>();
```

Once registered, inject  `IUnitOfWorkV2<TContext>` wherever needed.
Example: constructor injection in a controller:

```
public class UserController : ControllerBase
{
    private readonly IUnitOfWorkV2<UserContext> _uow;

    public UserController(IUnitOfWorkV2<UserContext> uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        var userList = _uow
            .GetQueryRepository<User, Guid>()
            .GetAll();

        return Ok(userList);
    }
}

```

With includes:

```
[HttpGet]
public ActionResult<IEnumerable<User>> Get()
{
    var userList = _uow
        .GetQueryRepository<User, Guid>()
        .GetAll("Address");

    return Ok(userList);
}
```

## Query capabilities

**Filtering**

All read methods support filtering via predicates:

```
var repo = _uow.GetQueryRepository<User, Guid>();

var activeUsers = repo.Get(u => u.IsActive);
```

Async:

```
var activeUsers = await repo.GetAsync(u => u.IsActive);
```

**Sorting**

Sorting can be applied via parameters or overloads (depending on your API shape). For example:

```
var sortedUsers = repo.Get(
    filter: u => u.IsActive,
    orderBy: q => q.OrderBy(u => u.LastName).ThenBy(u => u.FirstName));
```

**Pagination**

Pagination is supported out of the box:

```
var pagedUsers = repo.Get(
    filter: u => u.IsActive,
    orderBy: q => q.OrderBy(u => u.LastName),
    skip: 1,
    take: 20    
    );
```

Async:

```
var pagedUsers = repo.GetAsync(
    filter: u => u.IsActive,
    orderBy: q => q.OrderBy(u => u.LastName),
    skip: 1,
    take: 20    
    );
```

**Change tracking**

You can control whether EF Core tracks entities:

```
var trackedUsers = repo.GetAll(asNoTracking: true);
var untrackedUsers = repo.GetAll(asNoTracking: false);
```

## ISqlExecutor Overview

`ISqlExecutor` provides three core operations:
- Execute SQL commands (INSERT, UPDATE, DELETE)
- Query entities or DTOs
- Query a single entity or DTO

```
public interface ISqlExecutor
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> QuerySingleAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);
}
```

The interface is non‑generic, and each method is generic. This design allows you to query:
- EF Core entities
- DTOs
- Projections
- Scalar values (via DTO wrappers)

**EF Core Implementation**

Prakrishta.Data ships with an EF Core–backed implementation

```
public class EfCoreSqlExecutor : ISqlExecutor
{
    private readonly DbContext _context;

    public EfCoreSqlExecutor(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await _context.Set<T>()
            .FromSqlRaw(sql, parameters)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> QuerySingleAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await _context.Set<T>()
            .FromSqlRaw(sql, parameters)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }
}
```

**Registering ISqlExecutor**

Add the executor to your DI container

```
services.AddScoped<ISqlExecutor, EfCoreSqlExecutor>();
```

**Usage Examples**

1. Querying Entities with Raw SQL

```
var users = await sqlExecutor.QueryAsync<User>(
    "SELECT * FROM Users WHERE IsActive = 1"
);
```

2. Querying with Parameters (Safe SQL)

```
var users = await sqlExecutor.QueryAsync<User>(
    "SELECT * FROM Users WHERE Name = @name",
    new { name = "John" }
);
```

Parameters are automatically parameterized by EF Core, preventing SQL injection.

3. Querying DTOs

```
public class UserSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

var summaries = await sqlExecutor.QueryAsync<UserSummaryDto>(
    "SELECT Id, Name FROM Users WHERE IsActive = 1"
);
```

DTOs must have property names matching the SQL result set.

4. Querying a Single Row

```
var user = await sqlExecutor.QuerySingleAsync<User>(
    "SELECT * FROM Users WHERE Id = @id",
    new { id = 5 }
);
```

Returns null if no row matches.

5. Executing Commands (INSERT, UPDATE, DELETE)

```
var rows = await sqlExecutor.ExecuteAsync(
    "UPDATE Users SET IsActive = 0 WHERE LastLogin < @cutoff",
    new { cutoff = DateTime.UtcNow.AddMonths(-6) }
);
```

rows contains the number of affected rows.

6. Stored Procedure Execution

```
var results = await sqlExecutor.QueryAsync<UserSummaryDto>(
    "EXEC GetActiveUsers @role",
    new { role = "Admin" }
);
```

Works with SQL Server, PostgreSQL, MySQL, and SQLite (where supported).

**Security Considerations**

Prakrishta.Data encourages safe SQL usage by requiring parameters to be passed separately:

```
new { name = userInput }
```

This ensures:
- Automatic parameterization
- No string concatenation
- No SQL injection vulnerabilities
Consumers should avoid interpolated SQL strings.


## Specification pattern integration

To avoid repository method explosion and centralize query logic, Prakrishta.Data supports the Specification Pattern on top of the existing Unit of Work and repositories.

**Specification abstractions**

A simple base specification interface and implementation:

```
public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
    IList<Expression<Func<T, object>>>? IncludeProperties { get; }
    int? PageNumber { get; }
    int? PageSize { get; }
    bool IsPagingEnabled { get; }
    bool AsNoTracking { get; }
}

public abstract class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }
    public IList<Expression<Func<T, object>>>? IncludeProperties { get; protected set; } = []
    public int? PageNumber { get; protected set; }
    public int? PageSize { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }
    public bool AsNoTracking { get; protected set; } = true;


    protected void AddInclude(Expression<Func<T, object>> includeProperty)
    {
        if (IncludeProperties is null)
        {
            throw new InvalidOperationException("IncludeProperties collection is null.");
        }

        IncludeProperties.Add(includeProperty);
    }

    protected void ApplyPaging(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression) => OrderBy = orderByExpression;

    protected void TrackChanges() => AsNoTracking = false;

}
```

**Specification evaluator**

The evaluator translates a specification into an IQueryable<T>:

```
public static IQueryable<T> EvaluateQuery<T>(IQueryable<T> inputQuery,
    ISpecification<T> spec) where T : class
{
    IQueryable<T> query = inputQuery;

    if (spec.Criteria is not null)
        query = query.Where(spec.Criteria);

    foreach (var include in spec.IncludeProperties??[])
        query = query.Include(include);

    if (spec.PageNumber.HasValue && spec.PageSize.HasValue)
    {
        var skip = (spec.PageNumber.Value - 1) * spec.PageSize.Value;
        query = query.Skip(skip).Take(spec.PageSize.Value);
    }

    if (spec.AsNoTracking)
      query = query.AsNoTracking();

    return query;
}
```

**Specification-enabled repositories**

You can extend your existing repositories to support specification-based methods.
Query repository with specifications

```
public interface IQueryRepository<TEntity, TId> where TEntity : class, IAuditableBaseEntity<TId>
{
    Task<IReadOnlyList<TEntity?>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TEntity?> GetFirstAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
}
```

Implementation:

```
public class QueryRepository<TEntity, TId>(DbContext dbContext) : RepositoryBase<TEntity>(dbContext)
    , IQueryRepository<TEntity, TId> where TEntity : class, IAuditableBaseEntity<TId>
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity?>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationExecutor.EvaluateQuery<TEntity>(this.DbSet.AsQueryable(), specification);
        return await query.ToListAsync(cancellationToken);
    
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetFirstAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        var query = SpecificationExecutor.EvaluateQuery<TEntity>(this.DbSet.AsQueryable(), specification);
        return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
```

These methods are then naturally exposed via `IUnitOfWorkV2`:

```
var repo = _uow.GetQueryRepository<User, Guid>();
var users = await repo.ListAsync(specification);
```

**Example specifications**

List: active users with paging and includes

```
public class ActiveUsersWithAddressSpecification : Specification<User>
{
    public ActiveUsersWithAddressSpecification(int pageIndex, int pageSize)
    {
        Criteria = u => u.IsActive;

        AddInclude(u => u.Address);

        ApplyOrderBy(q => q.OrderBy(u => u.LastName).ThenBy(u => u.FirstName));

        ApplyPaging(pageIndex, pageSize);
    }
}
```

Usage:

```
[HttpGet]
public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers(
    int pageIndex = 1,
    int pageSize = 20)
{
    var spec = new ActiveUsersWithAddressSpecification(pageIndex, pageSize);

    var users = await _uow
        .GetQueryRepository<User, Guid>()
        .ListAsync(spec);

    return Ok(users);
}
```

Single record: user by id with includes:

```
public class UserByIdWithAddressSpecification : Specification<User>
{
    public UserByIdWithAddressSpecification(Guid userId)
    {
        Criteria = u => u.Id == userId;

        AddInclude(u => u.Address);

        TrackChanges(); // if you want tracked entity for updates
    }
}
```

Usage:

```
[HttpGet("{id:guid}")]
public async Task<ActionResult<User>> GetById(Guid id)
{
    var spec = new UserByIdWithAddressSpecification(id);

    var user = await _uow
        .GetQueryRepository<User, Guid>()
        .FirstOrDefaultAsync(spec);

    if (user is null)
        return NotFound();

    return Ok(user);
}
```

**Generic “by id” specification**

```
public class EntityByIdSpecification<T, TKey> : Specification<T>
    where T : class
{
    public EntityByIdSpecification(TKey id)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id);
        var equal = Expression.Equal(property, constant);

        Criteria = Expression.Lambda<Func<T, bool>>(equal, parameter);
    }
}
```

Usage:

```
var spec = new EntityByIdSpecification<User, Guid>(id);
var user = await _uow
    .GetQueryRepository<User, Guid>()
    .FirstOrDefaultAsync(spec);
```

**Putting it all together**

With **Prakrishta.Data**, your data access story becomes:
- Unit of Work for transaction boundaries and repository orchestration.
- Query and Persistence Repositories for clean separation of read and write concerns.
- Specification Pattern for expressive, reusable query logic that keeps repositories small and stable.
- DI integration for clean, testable application code.

The code base is licensed under open source, please feel free to use it in your project or edit as per your need.
